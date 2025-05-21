using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.IO;
using Ssit.IoC;

namespace Ssit.CrossX.Audio.Internal;

internal class MusicPlayer: IMusicPlayer, IMusicDataProvider, IDisposable
{
    private readonly Dictionary<string, MusicPlaylist> _playlists = new();
    private MusicPlaylist _currentPlaylist;
    
    private const int BufferLength = 88200 / 10;
    
    private readonly IFilesProvider _filesProvider;
    private readonly IEventSource _eventSource;
    private readonly IIoCContainer _iocContainer;
    private readonly IActionScheduler _scheduler;

    private readonly List<ISingleMusicPlayer> _musicPlayers = new ();

    public float Volume { get; set; } = 0.5f;
    
    public MusicPlayer(IFilesProvider filesProvider, IEventSource eventSource, IIoCContainer iocContainer, IActionScheduler scheduler)
    {
        _filesProvider = filesProvider;
        _eventSource = eventSource;
        _iocContainer = iocContainer;
        _scheduler = scheduler;

        _eventSource.Updating += EventSourceOnUpdating;
    }
    
    public IMusicPlayer RegisterPlaylist(string name, MusicPlaylist playlist)
    {
        _playlists.Add(name, playlist);
        return this;
    }

    public void ChangePlaylist(string name, int fadeTimeMs = 0, bool resetProgress = false)
    {
        var newPlaylist = _playlists[name];
        
        if (_currentPlaylist is not null)
        {
            if (ReferenceEquals(newPlaylist, _currentPlaylist))
                return;
            
            var currentPosition = GetSongPosition();
            _currentPlaylist.CurrentPosition = currentPosition;
        }

        _currentPlaylist = newPlaylist;

        if (resetProgress)
        {
            _currentPlaylist.CurrentSong = 0;
            _currentPlaylist.CurrentPosition = 0;
        }
        
        SwitchSong(_currentPlaylist.List[_currentPlaylist.CurrentSong], fadeTimeMs, _currentPlaylist.CurrentPosition);
    }

    public bool NextTrack(int fadeTimeMs = 0)
    {
        if (_currentPlaylist is null) return false;

        var song = GetNextSong();
        SwitchSong(song, fadeTimeMs);
        return true;
    }

    public bool PreviousTrack(int fadeTimeMs = 0)
    {
        if (_currentPlaylist is null) return false;
        
        var nextSong = _currentPlaylist.CurrentSong - 1;
        if (nextSong < 0)
        {
            nextSong += _currentPlaylist.List.Count;
        }

        _currentPlaylist.CurrentPosition = 0;
        _currentPlaylist.CurrentSong = nextSong;
        
        SwitchSong(_currentPlaylist.List[nextSong], fadeTimeMs);
        return true;
    }
    
    public Song GetNextSong()
    {
        var nextSong = _currentPlaylist.CurrentSong + 1;
        if (_currentPlaylist.List.Count <= nextSong)
        {
            nextSong %= _currentPlaylist.List.Count;
        }
        
        _currentPlaylist.CurrentPosition = 0;
        _currentPlaylist.CurrentSong = nextSong;

        return _currentPlaylist.List[nextSong];
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

    protected int GetSongPosition()
    {
        return _musicPlayers.LastOrDefault()?.Position ?? 0;
    }

    protected void SwitchSong(Song song, int fadeTime, int startPosition = 0)
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