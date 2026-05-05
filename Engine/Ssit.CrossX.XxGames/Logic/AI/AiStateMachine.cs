using System;
using System.Collections.Generic;

namespace Ssit.CrossX.XxGames.Logic.AI;

public class AiStateMachine<TObject>(TObject obj)
{
    public TObject Object { get; } = obj;
    
    public AiState<TObject> CurrentState { get; private set; }

    private readonly Dictionary<string, AiState<TObject>> _states = new();

    public void RegisterState(AiState<TObject> state) => _states.Add(state.Name, state);

    public void SetState(string stateName)
    {
        if (!_states.TryGetValue(stateName, out var state))
        {
            throw new Exception($"State {stateName} not found");
        }
        
        CurrentState?.Exit(this);
        CurrentState = state;
        CurrentState.Enter(this);
    }

    public void Update() => CurrentState?.Update(this);
}