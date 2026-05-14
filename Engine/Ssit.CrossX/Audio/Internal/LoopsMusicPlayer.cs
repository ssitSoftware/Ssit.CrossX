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

    private MusicPlaylist _currentPlaylist;
    private MultiSongDataProvider _currentMusicProvider;

    private bool _isActive = true;

    private string _currentPlaylistName = "";
    
    public LoopsMusicPlayer(IFilesProvider filesProvider, IIoCContainer iocContainer, IActionScheduler scheduler, IEventSource eventSource)
    {
        _filesProvider = filesProvider;
        _iocContainer = iocContainer;
        _scheduler = scheduler;
        
        eventSource.Paused += EventSourceOnPaused;
        eventSource.Resumed += EventSourceOnResumed;
    }

    private void EventSourceOnResumed()
    {
        if (_isActive)
            return;
        
        _isActive = true;
        
        if (!string.IsNullOrWhiteSpace(_currentPlaylistName))
        {
            var name = _currentPlaylistName;
            _currentPlaylistName = "";
            ChangePlaylist(name);
        }
    }

    private void EventSourceOnPaused()
    {
        foreach (var player in _players)
        {
            player.FadeOut(10);
        }
        
        if (_currentPlaylist != null && _currentMusicProvider != null)
        {
            _currentPlaylist.CurrentSong = _currentMusicProvider.CurrentSongIndex;
            _currentPlaylist.CurrentPosition = _currentMusicProvider.CurrentSongBlock;
        }
        
        _isActive = false;
        _currentPlaylist = null;
        _currentMusicProvider = null;
    }

    public IMusicPlayer RegisterPlaylist(string name, MusicPlaylist playlist)
    {
        _playlists.Add(name, playlist);
        return this;
    }

    public void ChangePlaylist(string name, int fadeTimeMs = 250, bool resetProgress = false)
    {
        if (!_isActive)
        {
            _currentPlaylistName = name;
            return;
        }
        
        if (!_playlists.TryGetValue(name, out var playlist)) return;

        if (ReferenceEquals(_currentPlaylist, playlist)) return;
        
        if (_currentPlaylist != null && _currentMusicProvider != null)
        {
            _currentPlaylist.CurrentSong = _currentMusicProvider.CurrentSongIndex;
            _currentPlaylist.CurrentPosition = _currentMusicProvider.CurrentSongBlock;
        }
        
        _currentPlaylist = playlist;
        _currentPlaylistName = name;
        
        var songs = playlist.List;

        Task.Run(() =>
        {
            _currentMusicProvider = new MultiSongDataProvider(_filesProvider, songs, _currentPlaylist.CurrentSong, _currentPlaylist.CurrentPosition);

            _scheduler.Schedule(() =>
            {
                foreach (var player in _players)
                {
                    player.FadeOut(fadeTimeMs);
                }

                var newPlayer = _iocContainer.IoCConstruct<ISingleMusicPlayer>();
                newPlayer.Name = name;
                
                _players.Add(newPlayer);
                newPlayer.Start(_currentMusicProvider, BufferLength, fadeTimeMs);
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
