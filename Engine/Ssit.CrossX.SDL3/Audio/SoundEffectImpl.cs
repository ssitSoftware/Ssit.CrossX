using OpenTK.Audio.OpenAL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.Content;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.SDL3.Audio;

internal class SoundEffectImpl: ISoundEffect, IInstanceCountingResource
{
    private readonly IIoCContainer _iocContainer;
    private int _bufferHandle;

    private readonly List<ISoundEffectInstance> _instances = new();

    public SoundEffectImpl(IIoCContainer iocContainer, Stream stream)
    {
        _iocContainer = iocContainer;

        var wav = WavLoader.LoadMonoWav(stream);
        
        _bufferHandle = AL.GenBuffer();

        unsafe
        {
            fixed (short* ptr = wav.buffer)
            {
                AL.BufferData(_bufferHandle, ALFormat.Mono16, 
                    (IntPtr)ptr, wav.buffer.Length * sizeof(short),
                    wav.sampleRate);
            }
        }
    }

    public ISoundEffectInstance CreateInstance() => _iocContainer.IoCConstruct<SoundEffectInstanceImpl>(this);

    public void PlayOnce(float volume = 1, float pitch = 1, ISoundEmitter emitter = null)
    {
        var instance = CreateInstance();
        instance.Emitter = emitter;
        
        _instances.Add(instance);
        
        instance.Finished += () =>
        {
            _instances.Remove(instance);
            instance.Dispose();
        };
        
        instance.Parameters = new SoundParameters
        {
            Volume = volume,
            Pitch = pitch
        };
        
        instance.Play();
    }
    
    public void Dispose()
    {
        foreach (var instance in _instances)
        {
            instance.Dispose();
        }
        _instances.Clear();
        
        if (_bufferHandle != 0)
        {
            AL.DeleteBuffer(_bufferHandle);
            _bufferHandle = 0;
        }
    }

    public Action<Guid> AddUser { get; set; }
    public Action<Guid> RemoveUser { get; set; }
    
    public int Buffer => _bufferHandle;
}