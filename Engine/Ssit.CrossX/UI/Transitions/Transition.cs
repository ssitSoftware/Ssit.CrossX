using System;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.UI.Transitions;

public abstract class Transition : ITransition
{
    public TransitionType ForTransitions { get; set; }
    
    public float Power { get; set; } = 1;
    public float ProgressMin { get; set; } = 0;
    public float ProgressMax { get; set; } = 1;
    
    private bool _applied;
    
    void ITransition.Apply(IRenderer2 renderer, TransitionType type, float progress)
    {
        if ((ForTransitions & type) == 0)
            return;
        
        renderer.StateManager.SaveState();
        
        progress = ProgressMin + (ProgressMax - ProgressMin) * progress;
        progress = MathF.Pow(progress, Power);
        
        OnApply(renderer, progress);

        _applied = true;
    }

    void ITransition.Finish(IRenderer2 renderer)
    {
        if (!_applied)
            return;
        
        renderer.StateManager.RestoreState();
        _applied = false;
    }
    
    protected abstract void OnApply(IRenderer2 renderer, float progress);
}