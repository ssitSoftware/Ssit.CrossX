using System;
using System.Collections.Generic;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public class SpriteGameObjectStateMachine<TObject, TStateObject> : SpriteInstance.IHandler, IUpdatable where TObject: SpriteGameObject2, TStateObject
{
    private readonly TObject _obj;
    public SteringStateMachine<TStateObject> SteringStateMachine { get; }
    
    private readonly Dictionary<string, string> _sequenceMapping = new();
    private readonly Dictionary<string, SteringState<TStateObject>> _steringStates = new();
    
    public SpriteGameObjectStateMachine(TObject obj)
    {
        _obj = obj;
        obj.AddUpdatableInternal(this);
        
        SteringStateMachine = new SteringStateMachine<TStateObject>(obj);
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
    
    public SpriteGameObjectStateMachine<TObject, TStateObject> RegisterState(SteringState<TStateObject> state)
    {
        var name =  state.Name;
        _steringStates.Add(name, state);
        return this;
    }

    public SpriteGameObjectStateMachine<TObject, TStateObject> MapSequence(string stateName, string sequenceName)
    {
        _sequenceMapping[stateName] = sequenceName;
        return this;
    }
    
    private void SteringStateMachineOnOnStateChanged(object sender, SteringState<TStateObject> state)
    {
        if (!_sequenceMapping.TryGetValue(state.Name, out var sequenceName))
        {
            sequenceName = state.Name;
        }
        
        _obj.Sprite.SetSequence(sequenceName);
    }

    void SpriteInstance.IHandler.OnSpriteEvent(SpriteInstance instance, SpriteInstance.Event @event)
    {
        SteringStateMachine.CurrentState?.Event(_obj, @event);
        _obj.CallSpriteEvent(@event);
    }

    void SpriteInstance.IHandler.OnSequenceFinished(SpriteInstance instance, string sequenceName, bool reverse)
    {
        SteringStateMachine.CurrentState?.SequenceFinished(_obj, sequenceName);
        _obj.CallSequenceFinished(sequenceName);
    }
    
    void IUpdatable.Update(float dt)
    {
        SteringStateMachine.CurrentState?.Update(_obj, dt);
    }
    
    void IUpdatable.FixedUpdate(float dt)
    {
        SteringStateMachine.CurrentState?.FixedUpdate(_obj, dt);
    }
}