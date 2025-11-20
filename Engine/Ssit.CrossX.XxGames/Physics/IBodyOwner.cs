using System;
using System.Numerics;

namespace Ssit.CrossX.XxGames.Physics;

public interface IBodyOwner: IDisposable
{
    event Action FixedUpdate;
    IBody Body { get; }
    
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
    
    void OnCollision(ICollider collider, ICollider other, Vector2 impact)
    {
    }
}