using System;

namespace Ssit.CrossX.XxGames.Physics;

public class Material : IMaterial
{
    public static IMaterial Default { get; } = new Material { Friction = 1, Bounce = 0, Sides = ColliderSides.All };
    public float Friction { get; set; }
    public float Bounce { get; set; }
    public ColliderSides Sides { get; set; }
    public int ColliderGroup { get; set; } = 1;

    public int Index { get; set; }

    public IMaterial Clone(int? newIndex = null)
    {
        var index = newIndex ?? Index;
        return new Material { Index = index, Bounce = Bounce, Sides = Sides, Friction = Friction, ColliderGroup = ColliderGroup };
    }
}