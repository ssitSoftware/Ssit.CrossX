using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic;

public interface ICamera
{
    Vector2 LookAt { get; }
    void SetPrimaryTarget(IBody body, Vector2 offset, float followFactor);
    void SetTemporaryTarget(IBody body, Vector2 offset, float followFactor, Action onFocused, TimeSpan returnAfter);
    void RemoveTemporaryTarget();
    void Update(float dt);
}