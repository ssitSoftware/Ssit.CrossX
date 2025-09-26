using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Stering;

public class SteringStateWithBehaviors<TObject>(string name, params SteringBehavior<TObject>[] behaviors) : SteringState<TObject>
{
    public override string Name => name;

    protected override void OnEnter(SteringStateMachine<TObject> sm, SteringState<TObject> previous)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Enter(sm, previous);
        }
    }
    
    protected override void OnExit(SteringStateMachine<TObject> sm, SteringState<TObject> next)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Exit(sm, next);
        }
    }

    protected override void OnUpdate(SteringStateMachine<TObject> sm, float dt)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.Update(sm, dt))
            {
                break;
            }
        }
    }
    
    protected override void OnFixedUpdate(SteringStateMachine<TObject> sm, float dt)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.FixedUpdate(sm, dt))
            {
                break;
            }
        }
    }

    protected override void OnEvent(SteringStateMachine<TObject> sm, IEvent @event)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.Event(sm, @event))
            {
                break;
            }
        }
    }

    protected override void OnSequenceFinished(SteringStateMachine<TObject> sm, string name)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.SequenceFinished(sm, name))
            {
                break;
            }
        }
    }
}