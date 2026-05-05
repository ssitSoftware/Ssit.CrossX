using Ssit.CrossX.XxGames.Logic.Steering;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ISteeringCharacter: ICharacter, IGameObject
{
    ISteeringInput SteeringInput { get; }
    CharacterSteeringParameters SteeringParameters { get; }
    ICharacterPhysicsValues PhysicsValues { get; }
    SteeringState<ISteeringCharacter> CurrentSteeringState { get; }
    void SetSteeringState(string name);
}