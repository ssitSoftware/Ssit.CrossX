using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.UI.Transitions;

public class TranslationTransition: Transition
{
    public Vector2 Offset { get; init; }
    
    protected override void OnApply(IRenderer2 renderer, float scale, float progress)
    {
        var offset = Offset * progress;
        renderer.StateManager.Translate(offset * scale);
    }
}

public class HideTransition: Transition
{
    protected override void OnApply( IRenderer2 renderer, float scale, float progress)
    {
        if (progress > 0)
        {
            renderer.StateManager.Scale(0);
        }
    }
}