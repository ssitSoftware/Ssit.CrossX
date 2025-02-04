using System.Collections.Generic;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;

namespace Ssit.CrossX.UI.Services;

internal class UiSoundsService(IContentManager contentManager) : IUiSounds
{
    private readonly Dictionary<string, ResourceHandle<ISoundEffect>> _sounds = new();

    ISoundEffect IUiSounds.this[string id]
    {
        get
        {
            if (_sounds.TryGetValue(id, out var sound))
            {
                return sound.Resource;
            }

            return null;
        }
    }

    IUiSounds IUiSounds.AddSound(string id, string path)
    {
        var sound = contentManager.Get<ISoundEffect>(path);
        _sounds.Add(id, sound);

        return this;
    }
}