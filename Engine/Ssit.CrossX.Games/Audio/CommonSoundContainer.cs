using System;
using System.Collections.Generic;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;

namespace Ssit.CrossX.Games.Audio;

internal class CommonSoundContainer(IContentManager contentManager) : ICommonSoundContainer, IDisposable
{
    private readonly Dictionary<string, (ResourceHandle<ISoundEffect>, float)> _sounds = new();

    public ICommonSoundContainer RegisterSound(string name, string path, float volume = 1)
    {
        var sound = contentManager.Get<ISoundEffect>(path);
        _sounds.Add(name, (sound, volume));
        return this;
    }

    public void Play(string name, float pitch = 1, ISoundEmitter emitter = null)
    {
        if (!_sounds.TryGetValue(name, out var sound))
        {
            return;
        }
        
        if (pitch == 0)
        {
            pitch = Random.Shared.NextSingle() / 5f + 0.9f;
        }
        
        sound.Item1.Resource.PlayOnce(volume: sound.Item2, emitter: emitter, pitch: pitch);
    }

    public void Dispose()
    {
        foreach (var sound in _sounds)
        {
            sound.Value.Item1.Dispose();
        }
        _sounds.Clear();
    }
}