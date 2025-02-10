using System;
using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public class StateManager : IStateManager, IRenderStateProvider
{
    public event Action<BlendMode> UpdateBlendMode;
    
    private readonly struct State(float scale, Vector2 offset, bool useGlowTextures, BlendMode blendMode)
    {
        public readonly float Scale = scale;
        public readonly Vector2 Offset = offset;
        public readonly bool UseGlowTextures = useGlowTextures;
        public readonly BlendMode BlendMode = blendMode;
    }
    
    private readonly Stack<State> _savedStates = new ();
    
    public void SaveState()
    {
        _savedStates.Push(_state);
    }

    public void RestoreState()
    {
        var blendMode = _state.BlendMode;
        _state = _savedStates.Pop();

        if (blendMode != _state.BlendMode)
        {
            UpdateBlendMode?.Invoke(_state.BlendMode);
        }
    }

    public void Reset()
    {
        _savedStates.Clear();
        _state = new State(1, Vector2.Zero, false, BlendMode.AlphaBlend);
    }

    public void PushScale(float scale)
    {
        _state = new State(_state.Scale * scale, _state.Offset, _state.UseGlowTextures, _state.BlendMode);
    }

    public void PushOffset(Vector2 offset)
    {
        _state = new State(_state.Scale, _state.Offset + offset * _state.Scale, _state.UseGlowTextures, _state.BlendMode);
    }
    
    public void PushBlendMode( BlendMode blendMode)
    {
        if(_state.BlendMode == blendMode) return;
        
        _state = new State(_state.Scale, _state.Offset, _state.UseGlowTextures, blendMode);
        UpdateBlendMode?.Invoke(_state.BlendMode);
    }

    public void PushUseGlowTextures(bool useGlowTextures)
    {
        _state = new State(_state.Scale, _state.Offset, useGlowTextures, _state.BlendMode);
    }

    private State _state = new(1, Vector2.Zero, false, BlendMode.AlphaBlend);

    public float Scale => _state.Scale;
    public Vector2 Offset => _state.Offset;
    public bool UseGlowTextures => _state.UseGlowTextures;
    public BlendMode BlendMode => _state.BlendMode;
}