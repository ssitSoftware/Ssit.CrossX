using System;

namespace Ssit.CrossX.Games.Logic.Stering;

public sealed class SteringStateMachine<TObject>(TObject obj)
{
    public TObject Object { get; } = obj;
    public SteringState<TObject> CurrentState { get; private set; }
    
    public event EventHandler<SteringState<TObject>> OnStateChanged;
    
    public void SetState(SteringState<TObject> state)
    {
        var prevState = CurrentState;
        CurrentState?.Exit(this, state);
        CurrentState = state;
        CurrentState.Enter(this, prevState);
        
        OnStateChanged?.Invoke(this, CurrentState);
    }
}