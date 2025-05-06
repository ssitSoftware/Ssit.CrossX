using Ssit.CrossX.Audio;

namespace Ssit.CrossX.SDL.Audio.Sdl;

public class SdlSoundManagerImpl: ISoundManager
{
    public void Dispose()
    {
    }

    public event Action MasterVolumeUpdated;
    public ISoundListener SoundListener { get; set; }
    public float MasterVolume { get; set; }
}