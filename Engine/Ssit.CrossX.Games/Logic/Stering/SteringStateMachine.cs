using System;

namespace Ssit.CrossX.Games.Logic.Stering;

public sealed class SteringStateMachine<TObject>(TObject obj)
{
    public TObject Object { get; } = obj;
    public SteringState<TObject> CurrentState { get; private set; }
    
    public event EventHandler<SteringState<TObject>> OnStateChanged;
    
    public void SetState(SteringState<TObject> state)
    {
        CurrentState?.Exit(Object);
        CurrentState = state;
        CurrentState.Enter(Object);
        
        OnStateChanged?.Invoke(this, CurrentState);
    }
}