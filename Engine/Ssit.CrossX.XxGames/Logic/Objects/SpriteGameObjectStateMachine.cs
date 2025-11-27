using System;
using System.Collections.Generic;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public class SpriteGameObjectStateMachine<TObject, TStateObject> : SpriteInstance.IHandler, IUpdatable where TObject: SpriteGameObject2, TStateObject
{
    private readonly TObject _obj;
    public SteringStateMachine<TStateObject> InternalStateMachine { get; }
    
    private readonly Dictionary<string, string> _sequenceMapping = new();
    private readonly Dictionary<string, SteringState<TStateObject>> _steringStates = new();
    
    public SpriteGameObjectStateMachine(TObject obj)
    {
        _obj = obj;
        obj.AddUpdatableInternal(this);
        
        InternalStateMachine = new SteringStateMachine<TStateObject>(obj);
        InternalStateMachine.OnStateChanged += OnStateChanged;

        obj.Sprite.Handler = this;
    }

    public void SetSteringState(string stateName)
    {
        if (stateName.Equals(InternalStateMachine.CurrentState?.Name ?? ""))
            return;
        
        if (!_steringStates.TryGetValue(stateName, out var state))
        {
            throw new Exception($"State {stateName} not found");
        }
        
        InternalStateMachine.SetState(state);
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
    
    private void OnStateChanged(object sender, SteringState<TStateObject> state)
    {
        if (!_sequenceMapping.TryGetValue(state.Name, out var sequenceName))
        {
            sequenceName = state.Name;
        }
        
        _obj.Sprite.SetSequence(sequenceName);
    }

    void SpriteInstance.IHandler.OnSpriteEvent(SpriteInstance instance, SpriteInstance.Event @event)
    {
        InternalStateMachine.CurrentState?.Event(_obj, @event);
        _obj.CallSpriteEvent(@event);
    }

    void SpriteInstance.IHandler.OnSequenceFinished(SpriteInstance instance, string sequenceName, bool reverse)
    {
        InternalStateMachine.CurrentState?.SequenceFinished(_obj, sequenceName);
        _obj.CallSequenceFinished(sequenceName);
    }
    
    void IUpdatable.Update(float dt)
    {
        InternalStateMachine.CurrentState?.Update(_obj, dt);
    }
    
    void IUpdatable.FixedUpdate(float dt)
    {
        InternalStateMachine.CurrentState?.FixedUpdate(_obj, dt);
    }
}