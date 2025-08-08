using Ssit.CrossX.Audio;

namespace Ssit.CrossX.SDL.Audio.Dummy;

public class DummySoundManagerImpl: ISoundManager
{
    private float _masterVolume;

    public void Dispose()
    {
    }

    public event Action MasterVolumeUpdated;
    public event Action Disposing;

    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            if(Math.Abs(_masterVolume - value) < float.Epsilon) return;
         
            _masterVolume = value;
            MasterVolumeUpdated?.Invoke();
        }
    }

    public ISoundListener SoundListener { get; set; }
}