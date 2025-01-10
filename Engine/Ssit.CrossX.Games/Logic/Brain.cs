using System;
using System.Collections.Generic;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Logic;

public abstract class Brain: IUpdatable
{
    private readonly Dictionary<string, State> _states = new();
    private State _currentState;
    private IGameObjectRenderer _renderer;

    protected Brain(IGameObjectRenderer renderer)
    {
        _renderer = renderer;
        _renderer.AnimationFinished += RendererOnAnimationFinished;
    }

    private void RendererOnAnimationFinished(string sequenceName, bool reverse)
    {
        _currentState?.SequenceFinished(sequenceName);
    }

    public string CurrentState { get; private set; }

    void IUpdatable.Update(float dt) => OnUpdate(dt);
    void IUpdatable.FixedUpdate(float dt) => OnFixedUpdate(dt);
    void IUpdatable.PostFixedUpdate() => OnPostFixedUpdate();

    protected virtual void OnUpdate(float dt)
    {
        _currentState?.Update(dt);
        SetSequence(CurrentState);
    }
    
    protected virtual void OnFixedUpdate(float dt)
    {
        _currentState?.FixedUpdate(dt);
        SetSequence(CurrentState);
    }
    
    protected virtual void OnPostFixedUpdate()
    {
        _currentState?.PostFixedUpdate();
        SetSequence(CurrentState);
    }

    protected void AddState(string name, State state)
    {
        _states.Add(name, state);
    }

    public void SetState(string state)
    {
        if (CurrentState == state)
            return;
        
        if (!_states.TryGetValue(state, out var instance))
            throw new InvalidOperationException();

        _currentState?.Leave();
        _currentState = instance;
        
        CurrentState = state;
        SetSequence(state);
        
        _currentState?.Enter();
    }

    protected virtual void SetSequence(string state)
    {
    }
}