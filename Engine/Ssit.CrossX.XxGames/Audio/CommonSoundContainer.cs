using System;
using System.Collections.Generic;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;

namespace Ssit.CrossX.XxGames.Audio;

internal class CommonSoundContainer(IContentManager contentManager) : ICommonSoundContainer, IDisposable
{
    private readonly Dictionary<string, (ResourceHandle<ISoundEffect>, float)> _sounds = new();

    public ICommonSoundContainer RegisterSound(string name, string path, float volume = 1)
    {
        var sound = contentManager.Get<ISoundEffect>(path);
        _sounds.Add(name, (sound, volume));
        return this;
    }

    public void Play(string name, float volume = 1, ISoundEmitter emitter = null)
    {
        if (!_sounds.TryGetValue(name, out var sound))
        {
            return;
        }
        
        sound.Item1.Resource.PlayOnce(volume: sound.Item2 * volume, emitter: emitter);
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