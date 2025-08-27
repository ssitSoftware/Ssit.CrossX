using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.UI.Transitions;

public interface ITransition
{
    void Apply(IRenderer2 renderer2, TransitionType type, float progress);
    void Finish(IRenderer2 renderer2);
}