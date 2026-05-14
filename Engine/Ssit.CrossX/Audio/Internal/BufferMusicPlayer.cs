using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.IO;
using Ssit.IoC;

namespace Ssit.CrossX.Audio.Internal;

public class BufferMusicPlayer: MusicPlayerBase, IMusicDataProvider, IUpdatable
{
    private readonly IFilesProvider _filesProvider;
    private readonly IIoCContainer _iocContainer;
    private readonly IActionScheduler _scheduler;

    private readonly List<ISingleMusicPlayer> _musicPlayers = new ();
    private bool _switching;
    
    public BufferMusicPlayer(IFilesProvider filesProvider, IIoCContainer iocContainer, IActionScheduler scheduler)
    {
        _filesProvider = filesProvider;
        _iocContainer = iocContainer;
        _scheduler = scheduler;
    }

    void IUpdatable.Update(float dt)
    {
        for (var idx =0; idx < _musicPlayers.Count; )
        {
            _musicPlayers[idx].Update(dt, out var finished);

            if (finished)
            {
                _musicPlayers[idx].Dispose();
                _musicPlayers.RemoveAt(idx);
                continue;
            }
            ++idx;
        }

        if (_musicPlayers.Count == 0 && !_switching)
        {
            NextTrack();
        }
    }

    protected override void OnDispose()
    {
        var players = _musicPlayers.ToArray();
        _musicPlayers.Clear();
        
        foreach (var player in players)
        {
            player.Dispose();
        }
    }

    protected override int GetSongPosition()
    {
        return _musicPlayers.LastOrDefault()?.Position ?? 0;
    }

    protected override void SwitchSong(Song song, int fadeTime, int startPosition = 0)
    {
        fadeTime =  Math.Max(50, fadeTime);
        _switching = true;

        Task.Run(() =>
        {
            var songStream = _filesProvider.Open(song.Path);
            var songProvider = new VorbisDataProvider(songStream);
            songProvider.Skip(startPosition, BufferLength);

            _scheduler.Schedule(() =>
            {
                _switching = false;

                foreach (var player in _musicPlayers)
                {
                    player.FadeOut(fadeTime);
                }

                var newPlayer = _iocContainer.IoCConstruct<ISingleMusicPlayer>(this);
                newPlayer.Name = song.Name;
                _musicPlayers.Add(newPlayer);
                newPlayer.Start(songProvider, BufferLength, fadeTime);
            });
        });
    }

    public VorbisDataProvider GetNext()
    {
        if (!IsCurrentPlaylistSingleSong) return null;
        var song = GetNextSong();
        var songStream = _filesProvider.Open(song.Path);
        return new VorbisDataProvider(songStream);
    }
}