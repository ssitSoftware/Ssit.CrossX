using System;
using System.Collections.Generic;
using Ssit.CrossX.Games.Logic.Stering;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic.Objects;

public class SpriteGameObjectStateMachine<TObject> : SpriteInstance.IHandler, IUpdatable where TObject: SpriteGameObject2
{
    public SteringStateMachine<TObject> SteringStateMachine { get; }
    private readonly Dictionary<string, string> _sequenceMapping = new();
    private readonly Dictionary<string, SteringState<TObject>> _steringStates = new();
    
    public SpriteGameObjectStateMachine(TObject obj)
    {
        obj.AddUpdatableInternal(this);
        
        SteringStateMachine = new SteringStateMachine<TObject>(obj);
        SteringStateMachine.OnStateChanged += SteringStateMachineOnOnStateChanged;

        obj.Sprite.Handler = this;
    }

    public void SetSteringState(string stateName)
    {
        if (!_steringStates.TryGetValue(stateName, out var state))
        {
            throw new Exception($"State {stateName} not found");
        }
        
        SteringStateMachine.SetState(state);
    }
    
    public SpriteGameObjectStateMachine<TObject> RegisterState(SteringState<TObject> state)
    {
        var name =  state.Name;
        _steringStates.Add(name, state);
        return this;
    }

    public SpriteGameObjectStateMachine<TObject> MapSequence(string stateName, string sequenceName)
    {
        _sequenceMapping[stateName] = sequenceName;
        return this;
    }
    
    private void SteringStateMachineOnOnStateChanged(SteringState<TObject> state)
    {
        if (!_sequenceMapping.TryGetValue(state.Name, out var sequenceName))
        {
            sequenceName = state.Name;
        }
        SteringStateMachine.Object.Sprite.SetSequence(sequenceName);
    }

    void SpriteInstance.IHandler.OnSpriteEvent(SpriteInstance instance, SpriteInstance.Event @event)
    {
        SteringStateMachine.CurrentState?.Event(@event);
        SteringStateMachine.Object.CallSpriteEvent(@event);
    }

    void SpriteInstance.IHandler.OnSequenceFinished(SpriteInstance instance, string sequenceName, bool reverse)
    {
        SteringStateMachine.CurrentState?.SequenceFinished(sequenceName);
        SteringStateMachine.Object.CallSequenceFinished(sequenceName);
    }
    
    void IUpdatable.Update(float dt)
    {
        SteringStateMachine.CurrentState?.Update(SteringStateMachine, dt);
    }
    
    void IUpdatable.FixedUpdate(float dt)
    {
        SteringStateMachine.CurrentState?.FixedUpdate(SteringStateMachine, dt);
    }
}