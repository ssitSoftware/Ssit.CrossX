using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class BlinkingLabelHandler<TLabel>: LabelHandler<TLabel> where TLabel: Label, IBlinkingView
{
    private float _currentTime = 0;

    private float VisibleTime => AttachedView.VisibleTime ?? 1f;
    private float HiddenTime => AttachedView.HiddenTime ?? 1f;
    
    public BlinkingLabelHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IActionDispatcher actionDispatcher, 
        IPaletteSource paletteSource = null) : base(parameters, fontsManager, actionDispatcher, paletteSource)
    {
    }

    public override void Update(float dt)
    {
        base.Update(dt);

        _currentTime += dt;
        _currentTime %= VisibleTime + HiddenTime;
    }

    public override void Draw(IRenderer2 renderer)
    {
        if (_currentTime < VisibleTime)
        {
            base.Draw(renderer);
        }
    }
}