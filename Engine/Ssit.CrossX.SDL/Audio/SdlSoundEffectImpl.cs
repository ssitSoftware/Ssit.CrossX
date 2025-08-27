using SDL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.SDL.Common;
using Ssit.IoC;
using static SDL.SDL3_mixer;

namespace Ssit.CrossX.SDL.Audio;

public unsafe class SdlSoundEffectImpl: ISoundEffect
{
    private readonly IIoCContainer _iocContainer;
    private readonly ISoundManager _soundManager;
    private readonly SdlHandle<Mix_Chunk> _chunk;
    
    private readonly List<ISoundEffectInstance> _instances = new();
    
    public SdlSoundEffectImpl(IIoCContainer iocContainer,  ISoundManager soundManager, Stream stream)
    {
        _iocContainer = iocContainer;
        _soundManager = soundManager;

        _soundManager.Disposing += Dispose;
        
        var path = Path.ChangeExtension(Path.GetTempPath(), Guid.NewGuid() + ".wav");
        using (var fileStream = File.Open(path, FileMode.Create))
        {
            stream.CopyTo(fileStream);
            fileStream.Flush();
        }
        
        var chunk =  Mix_LoadWAV(path);
        _chunk = new SdlHandle<Mix_Chunk>(chunk);
        
        File.Delete(path);
    }

    public void Dispose()
    {
        _soundManager.Disposing -= Dispose;
        
        foreach (var instance in _instances)
        {
            instance.Dispose();
        }
        _instances.Clear();
        
        if (_chunk.Pointer != null)
        {
            Mix_FreeChunk(_chunk.Pointer);
            _chunk.OnDisposed();
        }
    }

    public ISoundEffectInstance CreateInstance()
    {
        return _iocContainer.IoCConstruct<SdlSoundEffectInstanceImpl>(_chunk);
    }

    public void PlayOnce(float volume = 1, float pitch = 1, ISoundEmitter emitter = null)
    {
        var instance = CreateInstance();
        instance.Emitter = emitter;
        
        _instances.Add(instance);
        
        instance.Finished += InstanceOnFinished;
        
        instance.Parameters = new SoundParameters
        {
            Volume = volume,
            Pitch = pitch
        };
        
        instance.Play();
    }

    private void InstanceOnFinished(ISoundEffectInstance instance)
    {
        _instances.Remove(instance);
        instance.Dispose();
    }
}