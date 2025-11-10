using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer;

public struct Collision
{
    public ICollider Collider { get; }
    public Vector2 Impact { get; }
    public bool ByMovement { get; }

    public Collision(ICollider collider, Vector2 impact, bool byMovement)
    {
        Collider = collider;
        Impact = impact;
        ByMovement = byMovement;
    }
}