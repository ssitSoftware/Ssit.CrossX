using System;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Objects;

public class BulletObject : IBodyOwner
{
    public class Parameters
    {

    }

    public IBody Body { get; }
    public event Action FixedUpdate;

    public BulletObject(ISimulation simulation)
    {

    }

    public void Dispose()
    {
            
    }

    public void OnFixedUpdate(out bool cancelUpdate)
    {
        cancelUpdate = false;
    }

    public void OnPostFixedUpdate()
    {
            
    }

    public void OnUpdate(double time)
    {
            
    }

    public void Start()
    {
            
    }
}