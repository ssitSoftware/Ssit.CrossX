using System.Numerics;

namespace Ssit.CrossX.Games.Physics.Extensions;

public interface IMomentumReceiver
{
    void OnMomentumPassed(Vector2 offset);
}