using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Stering;

public abstract class SteringState<TObject>
{
    public abstract string Name { get; }
    
    internal void Enter(SteringStateMachine<TObject> sm, SteringState<TObject> previous) => OnEnter(sm, previous);
    internal void Exit(SteringStateMachine<TObject> sm, SteringState<TObject> next) => OnExit(sm, next);
    
    public void Update(SteringStateMachine<TObject> sm, float dt) => OnUpdate(sm, dt);
    public void FixedUpdate(SteringStateMachine<TObject> sm, float dt) => OnFixedUpdate(sm, dt);
    
    public void SequenceFinished(SteringStateMachine<TObject> sm, string name) => OnSequenceFinished(sm, name);
    public void Event(SteringStateMachine<TObject> sm, IEvent @event) => OnEvent(sm, @event);
    
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

    protected virtual void OnSequenceFinished(SteringStateMachine<TObject> sm, string name)
    {
    }

    protected virtual void OnEvent(SteringStateMachine<TObject> sm, IEvent @event)
    {
    }
}