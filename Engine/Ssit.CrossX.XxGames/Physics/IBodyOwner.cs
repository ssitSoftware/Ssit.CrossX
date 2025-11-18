

using System;

namespace Ssit.CrossX.XxGames.Physics;

public interface IBodyOwner: IDisposable
{
    void OnFixedUpdate(out bool cancelUpdate)
    {
        cancelUpdate = false;
    }

    void OnPostFixedUpdate()
    {
    }

    void OnUpdate(float time)
    {
    }

    void Start()
    {
    }

    IBody Body { get; }
    
    event Action FixedUpdate;
}