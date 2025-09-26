using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Stering;

public class SteringBehavior<TObject>
{
    internal void Enter(SteringStateMachine<TObject> sm, SteringState<TObject> previous) => OnEnter(sm, previous);
    internal void Exit(SteringStateMachine<TObject> sm, SteringState<TObject> next) => OnExit(sm, next);
    internal bool Event(IEvent @event) => OnEvent(@event);
    internal bool SequenceFinished(string name) => OnSequenceFinished(name);
    
    internal bool Update(SteringStateMachine<TObject> sm, float dt) => OnUpdate(sm, dt);
    internal bool FixedUpdate(SteringStateMachine<TObject> sm, float dt) => OnFixedUpdate(sm, dt);
    
    protected virtual bool OnFixedUpdate(SteringStateMachine<TObject> sm, float dt) => false;
    protected virtual bool OnUpdate(SteringStateMachine<TObject> sm, float dt) => false;
    
    protected virtual bool OnEvent(IEvent @event) => false;
    
    protected virtual bool OnSequenceFinished(string name) => false;
    
    protected virtual void OnEnter(SteringStateMachine<TObject> sm, SteringState<TObject> previous)
    {
    }

    protected virtual void OnExit(SteringStateMachine<TObject> sm, SteringState<TObject> next)
    {
    }
}