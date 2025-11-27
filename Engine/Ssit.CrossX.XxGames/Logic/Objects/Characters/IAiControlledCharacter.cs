using Ssit.CrossX.XxGames.Logic.AI;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface IAiControlledCharacter : ICharacter, IGameObject
{
    AiSteringInput AiSteringInput { get; }
    ISteringCharacter SteringCharacter { get; }
}