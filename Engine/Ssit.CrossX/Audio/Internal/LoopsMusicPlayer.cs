using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.IO;
using Ssit.IoC;

namespace Ssit.CrossX.Audio.Internal;

public class LoopsMusicPlayer : IMusicPlayer, IUpdatable, IDisposable
{
    private const int BufferLength = 88200 / 10;

    private readonly IFilesProvider _filesProvider;
    private readonly IIoCContainer _iocContainer;
    private readonly IActionScheduler _scheduler;

    private readonly Dictionary<string, MusicPlaylist> _playlists = new();
    private readonly List<ISingleMusicPlayer> _players = new();

    public LoopsMusicPlayer(IFilesProvider filesProvider, IIoCContainer iocContainer, IActionScheduler scheduler)
    {
        _filesProvider = filesProvider;
        _iocContainer = iocContainer;
        _scheduler = scheduler;
    }

    public IMusicPlayer RegisterPlaylist(string name, MusicPlaylist playlist)
    {
        _playlists.Add(name, playlist);
        return this;
    }

    public void ChangePlaylist(string name, int fadeTimeMs = 250, bool resetProgress = false)
    {
        if (!_playlists.TryGetValue(name, out var playlist)) return;
        
        var songs = playlist.List;

        Task.Run(() =>
        {
            var provider = new MultiSongDataProvider(_filesProvider, songs);

            _scheduler.Schedule(() =>
            {
                foreach (var player in _players)
                {
                    player.FadeOut(fadeTimeMs);
                }

                var newPlayer = _iocContainer.IoCConstruct<ISingleMusicPlayer>();
                _players.Add(newPlayer);
                newPlayer.Start(provider, BufferLength, fadeTimeMs);
            });
        });
    }

    void IUpdatable.Update(float dt)
    {
        for (var idx = 0; idx < _players.Count;)
        {
            _players[idx].Update(dt, out var finished);

            if (finished)
            {
                _players[idx].Dispose();
                _players.RemoveAt(idx);
                continue;
            }

            ++idx;
        }
    }

    public bool NextTrack(int fadeTimeMs = 0) => false;
    public bool PreviousTrack(int fadeTimeMs = 0) => false;

    public void Dispose()
    {
        var players = _players.ToArray();
        _players.Clear();

        foreach (var player in players)
        {
            player.Dispose();
        }
    }
}
