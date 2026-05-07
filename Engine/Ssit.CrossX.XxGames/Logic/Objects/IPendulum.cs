using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface IPendulum
{
    void AppendVelocity(float velocity);
    bool CanAttach(Aabb swingerAabb);
    void AttachObject(IPendulumSwinger swinger);
    void DetachObject(IPendulumSwinger swinger);
    Aabb GetBoundingBox();
    Vector2 AnchorPosition { get; }
}
