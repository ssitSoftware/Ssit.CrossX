using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Stering;

public abstract class SteringState<TObject>
{
    public abstract string Name { get; }
    
    internal void Enter(SteringStateMachine<TObject> sm, SteringState<TObject> previous) => OnEnter(sm, previous);
    internal void Exit(SteringStateMachine<TObject> sm, SteringState<TObject> next) => OnExit(sm, next);
    
    public void Update(SteringStateMachine<TObject> sm, float dt) => OnUpdate(sm, dt);
    public void FixedUpdate(SteringStateMachine<TObject> sm, float dt) => OnFixedUpdate(sm, dt);
    
    public void SequenceFinished(string name) => OnSequenceFinished(name);
    public void Event(IEvent @event) => OnEvent(@event);
    
    protected virtual void OnUpdate(SteringStateMachine<TObject> sm, float dt)
    {
    }
    
    protected virtual void OnFixedUpdate(SteringStateMachine<TObject> sm, float dt)
    {
    }

    protected virtual void OnEnter(SteringStateMachine<TObject> sm, SteringState<TObject> previous)
    {
    }

    protected virtual void OnExit(SteringStateMachine<TObject> sm, SteringState<TObject> next)
    {
    }

    protected virtual void OnSequenceFinished(string name)
    {
    }

    protected virtual void OnEvent(IEvent @event)
    {
    }
}