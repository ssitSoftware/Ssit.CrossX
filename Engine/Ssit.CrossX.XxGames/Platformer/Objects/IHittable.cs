using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Objects;

public interface IHittable
{
    bool Hit(HitKind kind, ICollider collider, Vector2 impact);
}