namespace Ssit.CrossX.XxGames.Logic.AI;

public abstract class AiState<TObject>
{
    public void Enter(AiStateMachine<TObject> sm) => OnEnter(sm);
    public void Exit(AiStateMachine<TObject> sm) => OnExit(sm);
    public void Update(AiStateMachine<TObject> sm) => OnUpdate(sm);

    protected virtual void OnUpdate(AiStateMachine<TObject> sm)
    {
    }

    protected virtual void OnEnter(AiStateMachine<TObject> sm)
    {
    }

    protected virtual void OnExit(AiStateMachine<TObject> sm)
    {
    }
}