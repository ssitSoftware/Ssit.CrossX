using System.Numerics;

namespace Ssit.CrossX.XxGames.Physics;

public delegate void CollisionDelegate(bool byMyMovement, ICollider other, Vector2 impact);