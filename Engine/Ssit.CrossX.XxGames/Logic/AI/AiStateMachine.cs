namespace Ssit.CrossX.XxGames.Logic.AI;

public class AiStateMachine<TObject>(TObject obj)
{
    public TObject Object { get; } = obj;
    public AiState<TObject> CurrentState { get; private set; }

    public void SetState(AiState<TObject> state)
    {
        CurrentState?.Exit(this);
        CurrentState = state;
        CurrentState.Enter(this);
    }

    public void Update() => CurrentState?.Update(this);
}