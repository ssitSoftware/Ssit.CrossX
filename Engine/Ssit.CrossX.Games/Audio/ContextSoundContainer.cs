using System;
using System.Collections.Generic;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;

namespace Ssit.CrossX.Games.Audio;

public class ContextSoundContainer: IDisposable
{
    public class Parameters
    {
        public ISoundEmitter Emitter;
    }
    
    private readonly IContentManager _contentManager;
    private readonly Parameters _parameters;

    private readonly Dictionary<(string, int), (ISoundEffectInstance, float)> _instances = new();
    private readonly List<ResourceHandle<ISoundEffect>> _sounds = new();
    
    public ContextSoundContainer(IContentManager contentManager, Parameters parameters)
    {
        _contentManager = contentManager;
        _parameters = parameters;
    }

    public ContextSoundContainer RegisterSound(string name, int material, string path, float volume = 1)
    {
        var sound = _contentManager.Get<ISoundEffect>(path);
        var instance = sound.Resource.CreateInstance();
        
        instance.Parameters = new SoundParameters
        {
            Volume = 1,
            Pitch = 1
        };
        instance.Emitter = _parameters.Emitter;
        
        _sounds.Add(sound);
        _instances.Add((name, material), (instance, volume));
        return this;
    }

    public void Play(string name, int material, float volume = 1)
    {
        if (!_instances.TryGetValue((name, material), out var instance))
        {
            if (!_instances.TryGetValue((name, -1), out instance))
            {
                return;
            }
        }

        if (instance.Item1.IsPlaying)
        {
            instance.Item1.Stop();
        }

        instance.Item1.Parameters = new SoundParameters
        {
            Pitch = 1,
            Volume = instance.Item2 * volume
        };

        instance.Item1.Play();
    }
    
    public void Dispose()
    {
        foreach (var instance in _instances)
        {
            instance.Value.Item1.Dispose();
        }
        _instances.Clear();
        
        foreach (var sound in _sounds)
        {
            sound.Dispose();
        }
        _sounds.Clear();
    }
}