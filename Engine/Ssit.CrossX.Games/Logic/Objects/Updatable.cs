using System.Collections.Generic;

namespace Ssit.CrossX.Games.Logic.Objects;

public abstract class Updatable : IUpdatable
{
    private readonly List<IUpdatable> _updatables = [];

    internal void AddUpdatableInternal( IUpdatable updatable) => _updatables.Add(updatable);
    
    protected void AddUpdatable( IUpdatable updatable ) => _updatables.Add(updatable);
    
    void IUpdatable.Update(float dt)
    {
        foreach (var updatable in _updatables)
        {
            updatable.Update(dt);
        }
        OnUpdate(dt);
    }
    
    void IUpdatable.FixedUpdate(float dt)
    {
        foreach (var updatable in _updatables)
        {
            updatable.FixedUpdate(dt);
        }
        OnFixedUpdate(dt);
    }

    void IUpdatable.PostFixedUpdate()
    {
        foreach (var updatable in _updatables)
        {
            updatable.PostFixedUpdate();
        }
        OnPostFixedUpdate();
    }

    void IUpdatable.PostUpdate()
    {
        foreach (var updatable in _updatables)
        {
            updatable.PostUpdate();
        }
        OnPostUpdate();
    }

    protected virtual void OnFixedUpdate(float dt)
    {
    }

    protected virtual void OnUpdate(float dt)
    {
    }
    
    protected virtual void OnPostFixedUpdate()
    {
    }
    
    protected virtual void OnPostUpdate()
    {
    }
}