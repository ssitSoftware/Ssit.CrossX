using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Stering;

public class SteringBehavior<TObject>
{
    internal void Enter(TObject obj) => OnEnter(obj);
    internal void Exit(TObject obj) => OnExit(obj);
    internal bool Event(TObject obj, IEvent @event) => OnEvent(obj, @event);
    internal bool SequenceFinished(TObject obj, string name) => OnSequenceFinished(obj, name);
    
    internal bool Update(TObject obj, float dt) => OnUpdate(obj, dt);
    internal bool FixedUpdate(TObject obj, float dt) => OnFixedUpdate(obj, dt);
    
    protected virtual bool OnFixedUpdate(TObject obj, float dt) => false;
    protected virtual bool OnUpdate(TObject obj, float dt) => false;
    
    protected virtual bool OnEvent(TObject obj, IEvent @event) => false;
    
    protected virtual bool OnSequenceFinished(TObject obj, string name) => false;
    
    protected virtual void OnEnter(TObject character)
    {
    }

    protected virtual void OnExit(TObject obj)
    {
    }
}