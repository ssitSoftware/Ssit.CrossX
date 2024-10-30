using System.Collections.Generic;

namespace Ssit.Pixel.Audio.Internal;

internal abstract class MusicPlayerBase: IMusicPlayer
{
    private readonly Dictionary<string, MusicPlaylist> _playlists = new();
    private MusicPlaylist _currentPlaylist;
    
    public void RegisterPlaylist(string name, MusicPlaylist playlist)
    {
        _playlists.Add(name, playlist);
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
    
    protected abstract int GetSongPosition();
    protected abstract void SwitchSong(Song song, int fadeTime, int startPosition = 0);
}