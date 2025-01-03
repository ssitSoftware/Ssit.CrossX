using System.Numerics;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Logic;

public interface IGameObjectController
{
    Vector2 GetMoveDirection();
    Vector2 GetAimDirection();
    
    ButtonState ShootButton { get; }
    ButtonState MeleeButton { get; }
    ButtonState ReloadButton { get; }
    ButtonState RollButton { get; }
}