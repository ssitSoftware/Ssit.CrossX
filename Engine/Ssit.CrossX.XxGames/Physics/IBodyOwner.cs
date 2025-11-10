

using System;

namespace Ssit.CrossX.XxGames.Physics;

public interface IBodyOwner: IDisposable
{
    void OnFixedUpdate(out bool cancelUpdate);
    void OnPostFixedUpdate();
    void OnUpdate(double time);
    void Start();
    IBody Body { get; }
    event Action FixedUpdate;
}