using System;
using System.Text;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.Utils;

public class GlobalStopwatchControl(IFontsManager fontsManager, IPaletteSource paletteSource = null)
{
    private readonly StringBuilder _text = new();
    private readonly TextRenderingContext _textRenderingContext = new();
    private float _scale = 1;
    private DateTime _lastTime = DateTime.MinValue;

    public GlobalStopwatchParameters Parameters { get; set; }

    public void Update()
    {
        var newStartTime = Parameters.StartTime?.Value;
        var nowTime = DateTime.UtcNow;
        var startTime = newStartTime ?? nowTime;

        if (!IsChanged(ref nowTime, ref newStartTime))
            return;

        FormatElapsedTime(nowTime - startTime);

        var font = GetFont(1f);
        _textRenderingContext.Reset();
        font.CalculateText(_text, TextSpacing.Normal, 0, _textRenderingContext);
    }

    public void Draw(IRenderer2 renderer, RectangleF bounds, float scale)
    {
        if (_text.Length == 0)
            return;

        var font = GetFont(scale);
        var textColor = Parameters.TextColor.GetColor(paletteSource, renderer);
        var outlineColor = Parameters.OutlineColor.GetColor(paletteSource, renderer);

        var padding = Parameters.Padding;
        var left = padding.Left?.Calculate(scale, bounds.Width) ?? 0;
        var top = padding.Top?.Calculate(scale, bounds.Height) ?? 0;
        var right = padding.Right?.Calculate(scale, bounds.Width) ?? 0;
        var bottom = padding.Bottom?.Calculate(scale, bounds.Height) ?? 0;
        bounds = new RectangleF(bounds.X + left, bounds.Y + top, bounds.Width - left - right, bounds.Height - top - bottom);

        renderer.TextRenderer.DrawText(
            font: font,
            text: _text,
            position: bounds,
            align: Parameters.Align,
            scale: GetTextScale(scale),
            color: textColor,
            outlineColor: outlineColor,
            context: _textRenderingContext);
    }

    private bool IsChanged(ref DateTime now, ref DateTime? currentStartTime)
    {
        if (!currentStartTime.HasValue)
        {
            return _text.Length == 0;
        }

        var startTime = currentStartTime.Value;

        var current = now - startTime;
        var last = _lastTime - startTime;

        var changed = false;

        if ((Parameters.TimeComponents & StopwatchComponents.Milliseconds) != 0)
            changed = (int)current.TotalMilliseconds != (int)last.TotalMilliseconds;
        else if ((Parameters.TimeComponents & StopwatchComponents.TenthSeconds) != 0)
            changed = (int)(current.TotalMilliseconds / 100) != (int)(last.TotalMilliseconds / 100);
        else if ((Parameters.TimeComponents & StopwatchComponents.Seconds) != 0)
            changed = (int)current.TotalSeconds != (int)last.TotalSeconds;
        else if ((Parameters.TimeComponents & StopwatchComponents.Minutes) != 0)
            changed = (int)current.TotalMinutes != (int)last.TotalMinutes;
        else if ((Parameters.TimeComponents & StopwatchComponents.Hours) != 0)
            changed = (int)current.TotalHours != (int)last.TotalHours;

        if (changed)
            _lastTime = now;

        return changed;
    }

    private void FormatElapsedTime(TimeSpan elapsed)
    {
        var hasHours = (Parameters.TimeComponents & StopwatchComponents.Hours) != 0;
        var hasMinutes = (Parameters.TimeComponents & StopwatchComponents.Minutes) != 0;
        var hasSeconds = (Parameters.TimeComponents & StopwatchComponents.Seconds) != 0;
        var hasMs = (Parameters.TimeComponents & StopwatchComponents.Milliseconds) != 0;
        var hasTenthSeconds = (Parameters.TimeComponents & StopwatchComponents.TenthSeconds) != 0;

        _text.Clear();

        var first = true;

        if (hasHours)
        {
            _text.AppendFormat("{0:D2}", (int)elapsed.TotalHours);
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

    private IFont GetFont(float scale)
    {
        var size = Parameters.Font.FontSize > 0 ? Parameters.Font.FontSize : 12;

        if (Parameters.Scaling == TextScaling.Default)
        {
            size = (int)MathF.Ceiling(size * scale);
        }

        var font = fontsManager.GetFont(Parameters.Font.FontFamily ?? "Default", size);
        _scale = (float)size / Math.Max(1, font.Size);
        return font;
    }

    private float GetTextScale(float scale) => Parameters.Scaling == TextScaling.Pixel ? scale : _scale;

    public void Reset()
    {
        if (_text.Length > 0)
        {
            _text.Clear();
        }
    }
}
