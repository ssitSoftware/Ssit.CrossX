using System;
using System.Collections.Generic;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;

namespace Ssit.CrossX.Games.Audio;

internal class CommonSoundContainer : ICommonSoundContainer, IDisposable
{
    private readonly IContentManager _contentManager;
    private readonly Dictionary<string, (ResourceHandle<ISoundEffect>, float)> _sounds = new();

    public CommonSoundContainer(IContentManager contentManager)
    {
        _contentManager = contentManager;
    }
    
    public ICommonSoundContainer RegisterSound(string name, string path, float volume = 1)
    {
        var sound = _contentManager.Get<ISoundEffect>(path);
        _sounds.Add(name, (sound, volume));
        return this;
    }

    public void Play(string name, ISoundEmitter emitter = null)
    {
        if (!_sounds.TryGetValue(name, out var sound))
        {
            return;
        }
        sound.Item1.Resource.PlayOnce(volume: sound.Item2, emitter: emitter);
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