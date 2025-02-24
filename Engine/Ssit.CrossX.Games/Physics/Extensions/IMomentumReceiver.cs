using System.Numerics;

namespace Ssit.CrossX.Games.Physics.Extensions;

public interface IMomentumReceiver
{
    void OnKineticallyMoved(Vector2 offset)
    {
    }

    Vector2 OffsetFactor => new(0.5f);
}