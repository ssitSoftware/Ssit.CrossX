using System.Collections.Generic;

namespace Ssit.Pixel.Audio.Internal;

public abstract class MusicPlayerBase: IMusicPlayer
{
    private readonly Dictionary<string, MusicPlaylist> _playlists = new();
    private MusicPlaylist _currentPlaylist;
    
    public void RegisterPlaylist(string name, MusicPlaylist playlist)
    {
        _playlists.Add(name, playlist);
    }

    public void ChangePlaylist(string name, int fadeTimeMs = 0, bool resetProgress = false)
    {
        if (_currentPlaylist is not null)
        {
            var currentPosition = GetSongPosition();
            _currentPlaylist.CurrentPosition = currentPosition;
        }
        
        _currentPlaylist = _playlists[name];

        if (resetProgress)
        {
            _currentPlaylist.CurrentSong = 0;
            _currentPlaylist.CurrentPosition = 0;
        }
        
        SwitchSong(_currentPlaylist.List[_currentPlaylist.CurrentSong], fadeTimeMs, _currentPlaylist.CurrentPosition);
    }

    public bool NextTrack(int fadeTimeMs = 0, bool loop = true)
    {
        if (_currentPlaylist is null) return false;
        
        var nextSong = _currentPlaylist.CurrentSong + 1;
        if (_currentPlaylist.List.Count <= nextSong)
        {
            if (loop)
            {
                nextSong %= _currentPlaylist.List.Count;
            }
            else
            {
                return false;
            }
        }
        
        _currentPlaylist.CurrentPosition = 0;
        _currentPlaylist.CurrentSong = nextSong;
        
        SwitchSong(_currentPlaylist.List[nextSong], fadeTimeMs);
        return true;
    }

    public bool PreviousTrack(int fadeTimeMs = 0, bool loop = true)
    {
        if (_currentPlaylist is null) return false;
        
        var nextSong = _currentPlaylist.CurrentSong - 1;
        if (nextSong < 0)
        {
            if (loop)
            {
                nextSong += _currentPlaylist.List.Count;
            }
            else
            {
                return false;
            }
        }

        _currentPlaylist.CurrentPosition = 0;
        _currentPlaylist.CurrentSong = nextSong;
        
        SwitchSong(_currentPlaylist.List[nextSong], fadeTimeMs);
        return true;
    }
    
    protected abstract int GetSongPosition();
    protected abstract void SwitchSong(Song song, int fadeTime, int startPosition = 0);
}