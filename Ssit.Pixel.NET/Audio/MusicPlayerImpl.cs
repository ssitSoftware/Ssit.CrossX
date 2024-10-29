using System;
using Ssit.Pixel.Audio;
using Ssit.Pixel.Audio.Internal;
using Ssit.Pixel.Core;
using Ssit.Pixel.IO;

namespace Ssit.Pixel.NET.Audio;

internal class MusicPlayerImpl : MusicPlayerBase, IDisposable
{
    private const int BufferLength = 88200 / 10;
    
    private readonly IFilesProvider _filesProvider;
    private readonly IEventSource _eventSource;
    
    private MusicPlayerInternal _currentSongPlayer;
    private MusicPlayerInternal _nextSongPlayer;

    private float _fadeTime = 0;
    private float _totalFadeTime = 0;
    
    public MusicPlayerImpl(IFilesProvider filesProvider, IEventSource eventSource)
    {
        _filesProvider = filesProvider;
        _eventSource = eventSource;
        
        _eventSource.Updating += EventSourceOnUpdating;
    }

    private void EventSourceOnUpdating(float dt)
    {
        bool hasNext = _fadeTime > 0 && _totalFadeTime > 0 && _nextSongPlayer is not null;

        if (_currentSongPlayer != null)
        {
            if (hasNext)
            {
                _fadeTime -= dt;
                
                var currentVolume = _fadeTime / _totalFadeTime;
                var nextVolume = (_totalFadeTime - _fadeTime) / _totalFadeTime;

                _currentSongPlayer.Volume = currentVolume;
                _nextSongPlayer.Volume = nextVolume;

                if (currentVolume < 0.00001f)
                {
                    _currentSongPlayer?.Dispose();
                    _currentSongPlayer = _nextSongPlayer;
                    _nextSongPlayer = null;

                    _fadeTime = 0;
                    _totalFadeTime = 0;
                }
            }
            else
            {
                _currentSongPlayer.Volume = 1;
                _fadeTime = 0;
            }
        }

        var currentPlaying = _currentSongPlayer?.Update() ?? false;
        _nextSongPlayer?.Update();
        
        if (!currentPlaying)
        {
            _currentSongPlayer?.Dispose();
            _currentSongPlayer = _nextSongPlayer;
            _nextSongPlayer = null;
            _fadeTime = 0;
        }

        if (_currentSongPlayer == null)
        {
            NextTrack();
        }
    }

    public void Dispose()
    {
        _currentSongPlayer?.Dispose();
        _currentSongPlayer = null;
        
        _nextSongPlayer?.Dispose();
        _nextSongPlayer = null;
    }

    protected override int GetSongPosition()
    {
        return _currentSongPlayer?.Position ?? 0;
    }

    protected override void SwitchSong(Song song, int fadeTime, int startPosition = 0)
    {
        if (_nextSongPlayer != null)
        {
            _currentSongPlayer?.Dispose();
            _currentSongPlayer = _nextSongPlayer;
            _nextSongPlayer = null;
        }

        var songStream = _filesProvider.Open(song.Path);
        var songProvider = new VorbisDataProvider(songStream);
        songProvider.Skip(startPosition, BufferLength);
        
        if (fadeTime == 0)
        {
            _currentSongPlayer?.Dispose();
            _currentSongPlayer = new MusicPlayerInternal(songProvider, BufferLength);
        }
        else
        {
            _nextSongPlayer = new MusicPlayerInternal(songProvider, BufferLength);
            _totalFadeTime = _fadeTime = fadeTime / 1000f;
        }
    }
}