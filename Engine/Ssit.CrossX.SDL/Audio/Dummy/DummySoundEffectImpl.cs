using Ssit.CrossX.Audio;

namespace Ssit.CrossX.SDL.Audio.Dummy;

public class DummySoundEffectImpl(Stream _): ISoundEffect
{
    public void Dispose()
    {
    }

    public ISoundEffectInstance CreateInstance() => new DummySoundEffectInstanceImpl();

    public void PlayOnce(float volume = 1, float pitch = 1, ISoundEmitter emitter = null)
    {
    }
}