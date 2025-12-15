using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.XxGames.Logic.Stering;

public abstract class SteringState<TObject>
{
    public abstract string Name { get; }
    
    internal void Enter(TObject obj) => OnEnter(obj);
    internal void Exit(TObject obj) => OnExit(obj);
    
    public void Update(TObject obj, float dt) => OnUpdate(obj, dt);
    public void FixedUpdate(TObject obj, float dt) => OnFixedUpdate(obj, dt);
    
    public void SequenceFinished(TObject obj, string name) => OnSequenceFinished(obj, name);
    public void Event(TObject obj, ISpriteEvent @event) => OnEvent(obj, @event);
    
    protected virtual void OnUpdate(TObject obj, float dt)
    {
    }
    
    protected virtual void OnFixedUpdate(TObject obj, float dt)
    {
    }

    protected virtual void OnEnter(TObject obj)
    {
    }

    protected virtual void OnExit(TObject obj)
    {
    }

    protected virtual void OnSequenceFinished(TObject obj, string name)
    {
    }

    protected virtual void OnEvent(TObject obj, ISpriteEvent @event)
    {
    }
}