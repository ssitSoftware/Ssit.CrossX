using SDL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Core;
using Ssit.CrossX.SDL.Common;

using static SDL.SDL3;
using static SDL.SDL3_mixer;

namespace Ssit.CrossX.SDL.Audio;

public unsafe class SdlSoundManagerImpl: ISoundManager, IUpdatable
{
    public event Action MasterVolumeUpdated;
    public event Action Disposing;
    public ISoundListener SoundListener { get; set; }

    public readonly SdlHandle<MIX_Mixer> MixerHandle;
    
    private readonly List<SdlSoundEffectInstanceImpl> _attachedInstances = new();
    
    public float MasterVolume
    {
        get => MIX_GetMasterGain(MixerHandle.Pointer);
        
        set
        {
            MIX_SetMasterGain(MixerHandle.Pointer, value);
            MasterVolumeUpdated?.Invoke();
        }
    }
    
    public SdlSoundManagerImpl()
    {
        MIX_Init();

        SDL_AudioSpec spec = new SDL_AudioSpec
        {
            format = SDL_AudioFormat.SDL_AUDIO_S16LE,
            channels = 2,
            freq = 44100
        };
        
        MixerHandle = new SdlHandle<MIX_Mixer>(MIX_CreateMixerDevice(SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK, &spec));
    }
    
    public void Dispose()
    {
        Disposing?.Invoke();

        if (MixerHandle.Pointer == null) return;
        
        MIX_DestroyMixer(MixerHandle.Pointer);
        MixerHandle.OnDisposed();
    }

    void IUpdatable.Update(float dt)
    {
        for ( var idx =0; idx < _attachedInstances.Count; )
        {
            var inst = _attachedInstances[idx];
            if (inst.CheckPlaying())
            {
                ++idx;
            }
        }
    }

    public void AttachInstance(SdlSoundEffectInstanceImpl sdlSoundEffectInstanceImpl)
    {
        _attachedInstances.Add(sdlSoundEffectInstanceImpl);   
    }

    public void DetachInstance(SdlSoundEffectInstanceImpl sdlSoundEffectInstanceImpl)
    {
        _attachedInstances.Remove(sdlSoundEffectInstanceImpl);
    }

    public void ForceFreeTracks()
    {
        foreach (var inst in _attachedInstances)
        {
            if (!inst.ForceFreeTrack()) continue;
            
            DetachInstance(inst);
            break;
        }
    }
}