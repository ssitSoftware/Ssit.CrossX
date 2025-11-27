using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ISteringCharacter: ICharacter, IGameObject
{
    ISteringInput SteringInput { get; }
    CharacterSteringParameters SteringParameters { get; }
    ICharacterPhysicsValues PhysicsValues { get; }
    SteringState<ISteringCharacter> CurrentSteringState { get; }
    void SetSteringState(string name);
}