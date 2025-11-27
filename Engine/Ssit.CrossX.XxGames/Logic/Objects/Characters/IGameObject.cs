using Ssit.CrossX.XxGames.Audio;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface IGameObject
{
    ContextSoundContainer SoundContainer { get; }
    TParameters Get<TParameters>(bool create = false) where TParameters : class;
}