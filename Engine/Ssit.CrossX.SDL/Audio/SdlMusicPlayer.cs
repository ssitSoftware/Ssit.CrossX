using SDL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3_mixer;

namespace Ssit.CrossX.SDL.Audio;

public unsafe class SdlMusicPlayer(SdlSoundManagerImpl soundManagerImpl): MusicPlayerBase
{
    private SdlHandle<MIX_Track> _currentMusic;
    private SdlHandle<MIX_Track> _oldMusic;
    
    private float _musicVolume = 1;
    private float _fadeInPosition;
    private float _fadeInSpeed = 1f;
    
    public override float Volume
    {
        get => _musicVolume;

        set
        {
            _musicVolume = value;
            UpdateVolume();
        }
    }

    private void UpdateVolume()
    {
        
    }

    public void Update(float dt)
    {
        if (_fadeInPosition < 1)
        {
            _fadeInPosition += dt * _fadeInSpeed;
            _fadeInPosition = Math.Min(1, _fadeInPosition);
            UpdateVolume();
        }
        else if (_oldMusic != null && _oldMusic.Pointer != null)
        {
            MIX_StopTrack(_oldMusic.Pointer, 0);
            MIX_DestroyTrack(_oldMusic.Pointer);
            _oldMusic.OnDisposed();
        }
    }
    
    protected override int GetSongPosition()
    {
        if (_currentMusic is null || _currentMusic.Pointer == null)
            return 0;

        return (int)MIX_GetTrackPlaybackPosition(_currentMusic.Pointer) / BufferLength;
    }

    protected override void SwitchSong(Song song, int fadeTime, int startPosition = 0)
    {
        var pos = startPosition;

        if (_oldMusic != null &&  _oldMusic.Pointer != null)
        {
            MIX_StopTrack(_oldMusic.Pointer, 0);
            MIX_DestroyTrack(_oldMusic.Pointer);
            _oldMusic.OnDisposed();
        }
        
        _oldMusic = _currentMusic;
        //_currentMusic = new SdlHandle<MIX_Track>(MIX_CreateTrack(soundManagerImpl.MixerHandle.Pointer));
        
        _fadeInPosition = 0;
        _fadeInSpeed = 1f / fadeTime;
        
        UpdateVolume();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        
        if (_oldMusic != null &&  _oldMusic.Pointer != null)
        {
            MIX_StopTrack(_oldMusic.Pointer, 0);
            MIX_DestroyTrack(_oldMusic.Pointer);
            _oldMusic.OnDisposed();
            _oldMusic = null;
        }
        
        if (_currentMusic != null &&  _currentMusic.Pointer != null)
        {
            MIX_StopTrack(_currentMusic.Pointer, 0);
            MIX_DestroyTrack(_currentMusic.Pointer);
            _currentMusic.OnDisposed();
            _currentMusic = null;
        }
    }
}