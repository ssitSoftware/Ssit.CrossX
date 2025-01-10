using Ssit.CrossX.Games.Physics.Dynamics;

namespace SampleGame.Game.Logic;

public interface IPhysicsObject
{
    Body Body { get; }
}