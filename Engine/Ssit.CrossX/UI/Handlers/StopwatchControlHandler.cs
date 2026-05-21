using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Components;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.UI.Handlers;

public class StopwatchControlHandler : ViewHandler<StopwatchControl>
{
    private readonly StopwatchComponent _stopwatch;
    private readonly StopwatchComponentParameters _componentParameters = new();

    public StopwatchControlHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IPaletteSource paletteSource = null)
        : base(parameters)
    {
        _stopwatch = new StopwatchComponent(fontsManager, paletteSource);
        _stopwatch.ComponentParameters = _componentParameters;
        
        _componentParameters.Font = AttachedView.Font;
        _componentParameters.TextColor = AttachedView.TextColor;
        _componentParameters.OutlineColor = AttachedView.OutlineColor;
        _componentParameters.Scaling = AttachedView.Scaling;
        _componentParameters.TimeTimeElements = AttachedView.TimeTimeElements;
        _componentParameters.StartTime = AttachedView.StartTime;
        _componentParameters.Padding = AttachedView.Padding ?? Thickness.Zero;
        _componentParameters.Align = AttachedView.Align ?? ContentAlign.Center | ContentAlign.VCenter;
        _componentParameters.ShouldDisplay = AttachedView?.Visible ?? true;
    }

    public override void Update(float dt)
    {
        base.Update(dt);

        if (AttachedView.Visible?.Value != true)
        {
            _stopwatch.Reset();
            return;
        }

        _stopwatch.Update();
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        _stopwatch.Draw(renderer, ScreenBounds, CurrentScale);
    }
}
