using System;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic;

public interface ICamera
{
    Vector2 LookAt { get; }
    void SetPrimaryTarget(Body body, Vector2 offset, float followFactor);
    void SetTemporaryTarget(Body body, Vector2 offset, float followFactor, Action onFocused, TimeSpan returnAfter);
    void Update(float dt);
}