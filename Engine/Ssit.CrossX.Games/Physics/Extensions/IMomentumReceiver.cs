using System.Numerics;

namespace Ssit.CrossX.Games.Physics.Extensions;

public interface IMomentumReceiver
{
    void OnKineticallyMoved(Vector2 offset)
    {
    }

    float OffsetFactor => 0.5f;
}