using System;
using System.Numerics;

namespace Ssit.CrossX.XxGames.Physics;

public interface IBody: IDisposable
{
    event Action Updated;
    event Action<Vector2> Friction;
    event Action<Vector2> Moved;

    float Mass { get; set; }
    bool IsActive { get; }
    bool IsKinematic { get; set; }
    Vector2 Position { get; set; }
    Vector2 Velocity { get; set; }
    Vector2 KinematicVelocity { get; set; }
    ICollider[] Colliders { get; }
    IBodyOwner Owner { get; }
    ISimulation Simulation { get; }
    float StepUpTolerance { get; set; }
    int UpdateOrder { get; set; }

    void AddColliders(params ICollider[] colliders);
    void Move(Vector2 offset);
    void KinematicMove(Vector2 offset, bool kinematicStandardMoveHybrid, IBody skipBody = null);
    void ApplyForce(Vector2 force);
    void LimitVelocity(float maxHorizontalVelocity, float maxVerticalVelocity);
    void LimitVelocity(float maxVelocity);
    void Touch();
    void DetachFromSimulation();
    void ReattachToSimulation();
    ICollider FindCollider(string name);
}