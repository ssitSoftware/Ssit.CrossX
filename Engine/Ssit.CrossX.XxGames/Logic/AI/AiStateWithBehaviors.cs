namespace Ssit.CrossX.XxGames.Logic.AI;

public class AiStateWithBehaviors<TObject>(AiBehavior<TObject>[] behaviors) : AiState<TObject>
{
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