using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class TextInputHandler(ViewHandler.CreateHandlerParameters parameters, IFontsManager fontsManager, IPaletteSource paletteSource = null) : ViewHandler<TextInput>(parameters), IFocusable, IInputConsumer
{
    public bool Focused { get; private set; }
    public bool DisableAllInput => false;

    public bool Enabled => AttachedView.Enabled?.Value ?? true;
    
    private bool _isActiveInput;
    private bool _hovered;
    private bool _pushed;
    
    private float _scale;
    
    protected float TextScale => AttachedView.Scaling == TextScaling.Pixel ? CurrentScale : _scale;

    public bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        throw new System.NotImplementedException();
    }

    public void SetFocus() => Focused = true;
    public bool ResetFocus() => Focused = false;
    public string UniqueId => AttachedView?.UniqueId;
    public bool SkipNavigation => false;
    
    protected IFont GetFont()
    {
        var size = AttachedView.Font?.FontSize ?? 12;

        if (AttachedView.Scaling == TextScaling.Default)
        {
            size = (int)MathF.Ceiling(size * CurrentScale);
        }
        
        var font = fontsManager.GetFont(AttachedView.Font?.FontFamily ?? "Default", size);

        _scale = (float)size / Math.Max(1, font.Size);
        return font;
    }
    
    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context)
    {
        throw new System.NotImplementedException();
    }

    public bool ProcessInput(IReadOnlyList<Pointer> pointers, IInputContext context)
    {
        throw new System.NotImplementedException();
    }

    public void CancelPointer(int pointerId, IInputContext context)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        
        var bgColor = GetColor(AttachedView.BackgroundColors, renderer);
        
        var sb = ScreenBounds;
        if (bgColor?.A > 0)
        {
            renderer.GeometryRenderer.FillRectangle(sb, bgColor.Value);
        }
        
        var frameColor = GetColor(AttachedView.FrameColors, renderer);
        
        if (frameColor?.A > 0)
        {
            renderer.GeometryRenderer.DrawFrame(sb, frameColor.Value, AttachedView.FrameThickness?.Calculate(CurrentScale, 1) ?? CurrentScale);
        }

        if (_isActiveInput)
        {
            frameColor = AttachedView.ActiveFrameColor?.GetColor(paletteSource, renderer);
            
            if (frameColor?.A > 0)
            {
                var thickness = AttachedView.ActiveFrameThickness?.Calculate(CurrentScale, 1) ?? CurrentScale;
                sb.Inflate(thickness + CurrentScale);
                
                renderer.GeometryRenderer.DrawFrame(sb, frameColor.Value, thickness);
            }
        }
    }

    private RgbaColor? GetColor(ButtonStateColors colors, IRenderer2 renderer) => colors.GetColor(renderer,
        paletteSource, _hovered, Focused, _pushed, Enabled, _isActiveInput);
}