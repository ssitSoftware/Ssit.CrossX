using Ssit.CrossX.Audio;

namespace Ssit.CrossX.SDL.Audio.Dummy;

public class DummySoundEffectInstanceImpl: ISoundEffectInstance
{
    public void Dispose()
    {
    }

    public event Action<ISoundEffectInstance> Finished;
    public ISoundEmitter Emitter { get; set; }
    public SoundParameters Parameters { get; set; }
    public bool IsPlaying => false;
    public void Play(bool loop = false)
    {
    }

    public void Stop()
    {
    }

    public void Pause()
    {
    }

    public void Resume()
    {
    }
}