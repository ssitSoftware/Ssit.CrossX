namespace Ssit.CrossX.Games.Logic.AI;

public abstract class AiBehavior<TObject>
{
    public void Enter(AiStateMachine<TObject> sm) => OnEnter(sm);
    public void Exit(AiStateMachine<TObject> sm) => OnExit(sm);
    public bool Update(AiStateMachine<TObject> sm) => OnUpdate(sm);
    
    protected virtual bool OnUpdate(AiStateMachine<TObject> sm) => false;
    
    protected virtual void OnEnter(AiStateMachine<TObject> sm)
    {
    }

    protected virtual void OnExit(AiStateMachine<TObject> sm)
    {
    }
}