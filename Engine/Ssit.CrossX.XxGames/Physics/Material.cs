using System;

namespace Ssit.CrossX.XxGames.Physics;

public class Material : IMaterial
{
    public static IMaterial Default { get; } = new Material { Friction = 0, Bounce = 0, Sides = ColliderSides.All };
    public float Friction { get; set; }
    public float Bounce { get; set; }
    public ColliderSides Sides { get; set; }
    public int ColliderGroup { get; set; }

    public Guid Guid { get; set; }

    public IMaterial Clone()
    {
        return new Material { Guid = Guid, Bounce = Bounce, Sides = Sides, Friction = Friction, ColliderGroup = ColliderGroup };
    }
}