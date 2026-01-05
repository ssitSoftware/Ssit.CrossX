using System;
using System.Threading.Tasks;
using SDL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.Core;
using Ssit.CrossX.IO;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3_mixer;

namespace Ssit.CrossX.SDL.Audio;

public unsafe class SdlMusicPlayer(IActionScheduler actionScheduler, IFilesProvider filesProvider): MusicPlayerBase
{
#if IOS || ANDROID
    private const float MaxVolume = MIX_MAX_VOLUME / 2f;
#else
    private const float MaxVolume = MIX_MAX_VOLUME;
#endif
    
    private SdlHandle<Mix_Music> _currentMusic;
    private SdlHandle<Mix_Music> _oldMusic;
    
    public override float Volume
    {
        get
        {
            var vol = Mix_VolumeMusic(-1);
            return vol  / MaxVolume;
        }
        
        set
        {
            var vol = (int) (value * MaxVolume);
            Mix_VolumeMusic(vol);
        }
    }
    
    protected override int GetSongPosition()
    {
        if (_currentMusic.Pointer == null)
            return 0;
        
        return (int)(Mix_GetMusicPosition(_currentMusic.Pointer) * 10000);
    }

    protected override void SwitchSong(Song song, int fadeTime, int startPosition = 0)
    {
        var pos = startPosition / 10000f;

        if (_oldMusic != null &&  _oldMusic.Pointer != null)
        {
            Mix_FreeMusic(_oldMusic.Pointer);
        }
        
        _oldMusic = _currentMusic;
        _currentMusic = new SdlHandle<Mix_Music>(Mix_LoadMUS(filesProvider.GetPhisicalFilePath(song.Path)));
        
        Mix_FadeOutMusic(fadeTime / 2);

        if (_currentMusic.Pointer == null)
        {
            Task.Delay(TimeSpan.FromMilliseconds(fadeTime / 2))
                .ContinueWith(t => actionScheduler.Schedule(Mix_HaltMusic));
        }
        else
        {
            Task.Delay(TimeSpan.FromMilliseconds(fadeTime / 2))
                .ContinueWith(t =>
                    actionScheduler.Schedule(() => Mix_FadeInMusicPos(_currentMusic.Pointer, -1, fadeTime / 2, pos)));
        }

        Task.Delay(TimeSpan.FromMilliseconds(fadeTime * 2)).ContinueWith(
            t => actionScheduler.Schedule(() =>
            {
                if (_oldMusic != null && _oldMusic.Pointer != null)
                {
                    Mix_FreeMusic(_oldMusic.Pointer);
                    _oldMusic.OnDisposed();
                    _oldMusic = null;
                }
            }));
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        
        if (_oldMusic != null &&  _oldMusic.Pointer != null)
        {
            Mix_FreeMusic(_oldMusic.Pointer);
            _oldMusic.OnDisposed();
            _oldMusic = null;
        }
        
        if (_currentMusic != null &&  _currentMusic.Pointer != null)
        {
            Mix_FreeMusic(_currentMusic.Pointer);
            _currentMusic.OnDisposed();
            _currentMusic = null;
        }
    }
}