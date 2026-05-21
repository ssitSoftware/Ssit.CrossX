using System;
using System.Text;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Views;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.UI.Handlers;

public class StopwatchControlHandler : ViewHandler<StopwatchControl>
{
    private readonly IFontsManager _fontsManager;
    private readonly IPaletteSource _paletteSource;
    private readonly IColorSource _colorSource;
    private readonly StringBuilder _text = new();
    private readonly TextRenderingContext _textRenderingContext = new();
    private float _scale = 1;

    private float TextScale => AttachedView.Scaling == TextScaling.Pixel ? CurrentScale : _scale;

    private DateTime _lastTime = DateTime.MinValue;
    
    public StopwatchControlHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IPaletteSource paletteSource = null)
        : base(parameters)
    {
        _fontsManager = fontsManager;
        _paletteSource = paletteSource;
        _colorSource = parameters.Parent?.GetParent<IColorSource>(true);
    }

    private bool IsChanged(ref DateTime now)
    {
        if (AttachedView.StartTime?.Value == null)
        {
            return _text.Length == 0;
        }
        
        var components = AttachedView.TimeComponents;
        var startTime = AttachedView.StartTime!.Value!.Value;
        
        var current = now - startTime;
        var last = _lastTime - startTime;

        var changed = false;
        if ((components & StopwatchComponents.Milliseconds) != 0)
        {
            changed = (int)current.TotalMilliseconds != (int)last.TotalMilliseconds;
        }
        else if ((components & StopwatchComponents.TenthSeconds) != 0)
        {
            changed = (int)(current.TotalMilliseconds / 100) != (int)(last.TotalMilliseconds / 100);
        }
        else if ((components & StopwatchComponents.Seconds) != 0)
        {
            changed = (int)current.TotalSeconds != (int)last.TotalSeconds;
        }
        else if ((components & StopwatchComponents.Minutes) != 0)
        {
            changed = (int)current.TotalMinutes != (int)last.TotalMinutes;
        }
        else if ((components & StopwatchComponents.Hours) != 0)
        {
            changed = (int)current.TotalHours != (int)last.TotalHours;
        }

        if (changed)
        {
            _lastTime = now;
        }

        return changed;
    }
    
    public override void Update(float dt)
    {
        base.Update(dt);
        
        if (AttachedView.Visible?.Value != true)
        {
            return;
        }
        
        var nowTime = DateTime.UtcNow;
        
        if (!IsChanged(ref nowTime))
        {
            return;
        }
        
        FormatElapsedTime(nowTime);

        var font = GetFont();
        
        _textRenderingContext.Reset();
        font.CalculateText(_text, TextSpacing.Normal, 0, _textRenderingContext);
    }

    private void FormatElapsedTime(DateTime nowTime)
    {
        var startTime = AttachedView.StartTime?.Value ?? nowTime;
        
        var elapsed =  nowTime  - startTime;
        var components = AttachedView.TimeComponents;
        
        var hasHours = (components & StopwatchComponents.Hours) != 0;
        var hasMinutes = (components & StopwatchComponents.Minutes) != 0;
        var hasSeconds = (components & StopwatchComponents.Seconds) != 0;
        var hasMs = (components & StopwatchComponents.Milliseconds) != 0;
        var hasTenthSeconds = (components & StopwatchComponents.TenthSeconds) != 0;
        
        _text.Clear();

        var first = true;

        if (hasHours)
        {
            _text.AppendFormat("{0:00}", (int)elapsed.TotalHours);
            first = false;
        }

        if (hasMinutes)
        {
            if (!first) _text.Append(':');
            var minutes = hasHours ? elapsed.Minutes : (int)elapsed.TotalMinutes;
            _text.AppendFormat("{0:D2}", minutes);
            first = false;
        }

        if (hasSeconds)
        {
            if (!first) _text.Append(':');
            var seconds = hasHours || hasMinutes ? elapsed.Seconds : (int)elapsed.TotalSeconds;
            _text.AppendFormat("{0:D2}", seconds);
            first = false;
        }

        if (hasMs)
        {
            if (!first) _text.Append('.');
            var ms = hasHours || hasMinutes || hasSeconds ? elapsed.Milliseconds : (int)elapsed.TotalMilliseconds;
            _text.AppendFormat("{0:D3}", ms);
        }
        else if (hasTenthSeconds)
        {
            if (!first) _text.Append('.');
            var tenths = hasHours || hasMinutes || hasSeconds ? elapsed.Milliseconds / 100 : (int)(elapsed.TotalMilliseconds / 100) % 10;
            _text.AppendFormat("{0:D1}", tenths);
        }
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);

        var font = GetFont();
        var textColor = AttachedView.TextColor.GetColor(_paletteSource, renderer, _colorSource);
        var outlineColor = AttachedView.OutlineColor.GetColor(_paletteSource, renderer, _colorSource);

        renderer.TextRenderer.DrawText(
            font: font,
            text: _text,
            position: ScreenBounds,
            align: ContentAlign.Center | ContentAlign.VCenter,
            scale: TextScale,
            color: textColor,
            outlineColor: outlineColor,
            context: _textRenderingContext);
    }

    private IFont GetFont()
    {
        var size = AttachedView.Font.FontSize > 0 ? AttachedView.Font.FontSize : 12;

        if (AttachedView.Scaling == TextScaling.Default)
        {
            size = (int)MathF.Ceiling(size * CurrentScale);
        }

        var font = _fontsManager.GetFont(AttachedView.Font.FontFamily ?? "Default", size);
        _scale = (float)size / Math.Max(1, font.Size);
        return font;
    }
}
