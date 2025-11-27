namespace Ssit.CrossX.XxGames.Logic.AI;

public class AiStateWithBehaviors<TObject>(string name, AiBehavior<TObject>[] behaviors) : AiState<TObject>
{
    public override string Name => name;
    
    protected override void OnEnter(AiStateMachine<TObject> sm)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Enter(sm);
        }
    }
    
    protected override void OnExit(AiStateMachine<TObject> sm)
    {
        foreach (var behavior in behaviors)
        {
            behavior.Exit(sm);
        }
    }

    protected override void OnUpdate(AiStateMachine<TObject> sm)
    {
        foreach (var behavior in behaviors)
        {
            if (behavior.Update(sm))
            {
                break;
            }
        }
    }
}