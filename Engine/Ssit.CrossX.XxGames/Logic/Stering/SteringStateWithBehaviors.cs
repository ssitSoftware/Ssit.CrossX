using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.XxGames.Logic.Stering;

public class SteringStateWithBehaviors<TObject>(string name, params SteringBehavior<TObject>[] behaviors) : SteringState<TObject>
{
    public override string Name => name;

    protected override void OnEnter(TObject obj)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Enter(obj);
        }
    }
    
    protected override void OnExit(TObject obj)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Exit(obj);
        }
    }

    protected override void OnUpdate(TObject obj, float dt)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.Update(obj, dt))
            {
                break;
            }
        }
    }
    
    protected override void OnFixedUpdate(TObject obj, float dt)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.FixedUpdate(obj, dt))
            {
                break;
            }
        }
    }

    protected override void OnEvent(TObject obj, ISpriteEvent @event)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.Event(obj, @event))
            {
                break;
            }
        }
    }

    protected override void OnSequenceFinished(TObject obj, string sequenceName)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.SequenceFinished(obj, sequenceName))
            {
                break;
            }
        }
    }
}