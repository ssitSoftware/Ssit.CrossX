using Ssit.CrossX.Audio;

namespace Ssit.CrossX.Games.Audio;

public interface ICommonSoundContainer
{
    void Play(string name, ISoundEmitter emitter = null);
    ICommonSoundContainer RegisterSound(string name, string path, float volume = 1);
}