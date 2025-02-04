using Ssit.CrossX.Audio;

namespace Ssit.CrossX.UI.Services;

public interface IUiSounds
{
    ISoundEffect this[string id] { get; }

    IUiSounds AddSound(string id, string path);
}