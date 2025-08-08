using SDL;
using Ssit.CrossX.Audio;

using static SDL.SDL3_mixer;

namespace Ssit.CrossX.SDL.Audio.Sdl;

public unsafe class SdlSoundManagerImpl: ISoundManager
{
    public event Action MasterVolumeUpdated;
    public event Action Disposing;
    public ISoundListener SoundListener { get; set; }

    public float MasterVolume
    {
        get
        {
            var  vol =  Mix_MasterVolume(-1);
            return vol / (float)MIX_MAX_VOLUME;
        }
        set
        {
            var vol = (int) (value * MIX_MAX_VOLUME);
            Mix_MasterVolume(vol);
            MasterVolumeUpdated?.Invoke();
        }
    }
    
    public event Action<int> ChannelIntercepted;
    
    public SdlSoundManagerImpl()
    {
        Mix_Init(MIX_INIT_OGG);

        var ch =  Mix_AllocateChannels(32);
        
        var audioSpec = new SDL_AudioSpec
        { 
            channels =  2,
            freq  = 44100,
            format =  SDL_AudioFormat.SDL_AUDIO_S16LE
        };

        if (!Mix_OpenAudio(0, &audioSpec))
        {
            throw  new InvalidProgramException("Failed to open audio");
        }
    }
    
    public void Dispose()
    {
        Disposing?.Invoke();
        //Mix_CloseAudio();
        Mix_Quit();
    }

    public void OnChannelIntercepted(int channel)
    {
        ChannelIntercepted?.Invoke(channel);
    }
}