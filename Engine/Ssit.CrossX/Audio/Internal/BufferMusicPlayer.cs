using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.IO;
using Ssit.IoC;

namespace Ssit.CrossX.Audio.Internal;

public class BufferMusicPlayer: MusicPlayerBase, IMusicDataProvider
{
    private const int BufferLength = 88200 / 10;
    
    private readonly IFilesProvider _filesProvider;
    private readonly IEventSource _eventSource;
    private readonly IIoCContainer _iocContainer;
    private readonly IActionScheduler _scheduler;

    private readonly List<ISingleMusicPlayer> _musicPlayers = new ();

    public override float Volume { get; set; } = 0.5f;
    
    public BufferMusicPlayer(IFilesProvider filesProvider, IEventSource eventSource, IIoCContainer iocContainer, IActionScheduler scheduler)
    {
        _filesProvider = filesProvider;
        _eventSource = eventSource;
        _iocContainer = iocContainer;
        _scheduler = scheduler;

        _eventSource.Updating += EventSourceOnUpdating;
    }

    private void EventSourceOnUpdating(float dt)
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
    }

    public void Dispose()
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

        Task.Run(() =>
        {
            var songStream = _filesProvider.Open(song.Path);
            var songProvider = new VorbisDataProvider(songStream);
            songProvider.Skip(startPosition, BufferLength);
            
            _scheduler.Schedule(() =>
            {
                foreach (var player in _musicPlayers)
                {
                    player.FadeOut(fadeTime);
                }
                        
                var newPlayer = _iocContainer.IoCConstruct<ISingleMusicPlayer>(this);
                _musicPlayers.Add(newPlayer);
                newPlayer.Start(songProvider, BufferLength, fadeTime);
            });
        });
    }

    public VorbisDataProvider GetNext()
    {
        var song = GetNextSong();
        var songStream = _filesProvider.Open(song.Path);
        return new VorbisDataProvider(songStream);
    }
}