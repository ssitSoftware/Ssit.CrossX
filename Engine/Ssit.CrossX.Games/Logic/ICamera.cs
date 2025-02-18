using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic;

public interface ICamera
{
    Vector2 LookAt { get; }
    void SetTarget(Body body, Vector2 offset);
    void Update(float dt);
}