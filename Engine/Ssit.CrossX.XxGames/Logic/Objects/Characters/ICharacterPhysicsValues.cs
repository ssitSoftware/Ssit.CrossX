namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ICharacterPhysicsValues
{
    float RunSpeed { get; }
    float Acceleration { get; }
    float WalkSpeed { get; }
    float JumpFactor { get; }
    float JumpVelocity { get; }
    float PushPullVelocity { get; }
    float JumpOfSpeed { get; }
}