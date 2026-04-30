using Ssit.CrossX.Audio;
using Ssit.CrossX.XxGames.Audio;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface IGameObject
{
    ICommonSoundContainer CommonSoundContainer { get; }
    ContextSoundContainer SoundContainer { get; }
    TParameters Get<TParameters>(bool create = false) where TParameters : class;
}