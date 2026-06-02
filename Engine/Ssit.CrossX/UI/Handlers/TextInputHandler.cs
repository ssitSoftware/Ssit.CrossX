using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.Text;
using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class TextInputHandler(
    ViewHandler.CreateHandlerParameters parameters,
    IFontsManager fontsManager,
    PageInputContext pageInputContext,
    IUiSounds uiSounds,
    INativeTextInputService nativeTextInputService,
    IInputCoordinateSystem inputCoordinateSystem,
    IPaletteSource paletteSource = null)
    : ViewHandler<TextInput>(parameters), IFocusable, IInputConsumer, INativeTextInputConsumer
{
    public bool Focused { get; private set; }
    public bool DisableAllInput => false;

    public bool Enabled => AttachedView.Enabled?.Value ?? true;

    private bool _isActiveInput;
    private bool _hovered;
    private bool _pushed;
    private int? _currentPointerId;
    private float _scale;

    private float _cursorPositionInPixels;
    
    private  INativeTextInput _currentTextInput;
    
    private int _cursorPosition;

    protected float TextScale => AttachedView.Scaling == TextScaling.Pixel ? CurrentScale : _scale;

    private readonly TextRenderingContext _textRenderingContext = new();
    
    private string _currentText;

    private bool _wasActiveInput;
    
    public override void Init()
    {
        base.Init();
        _currentText = AttachedView.Text?.ToString() ?? "";
    }

    public bool OnUiButton(UiButton button, IInputContext context)
    {
        if (_wasActiveInput)
        {
            if (button is UiButton.Back or UiButton.MenuOrBack)
            {
                Deactivate();
                return true;
            }

            return false;
        }
        
        FocusDirection focusDirection = FocusDirection.None;
        
        switch (button)
        {
            case UiButton.Left:
                focusDirection = FocusDirection.Left;
                break;

            case UiButton.Right:
                focusDirection = FocusDirection.Right;
                break;

            case UiButton.Up:
                focusDirection = FocusDirection.Up;
                break;

            case UiButton.Down:
                focusDirection = FocusDirection.Down;
                break;

            case UiButton.Select:
                if (Enabled)
                {
                    if (!pageInputContext.ShowFocus)
                    {
                        uiSounds[UiSounds.ItemNavigateSound]?.PlayOnce();
                        pageInputContext.ShowFocus = true;
                        return false;
                    }
                    (uiSounds[UiSounds.ExecuteSound] ?? uiSounds[UiSounds.ButtonReleaseSound])?.PlayOnce();
                    Activate();
                }
                break;
        }

        if (focusDirection != FocusDirection.None)
        {
            if (!pageInputContext.ShowFocus)
            {
                uiSounds[UiSounds.ItemNavigateSound]?.PlayOnce();
                pageInputContext.ShowFocus = true;
                context.Focus(this, this);
                return true;
            }

            if (context.MoveFocus(focusDirection, this))
            {
                uiSounds[UiSounds.ItemNavigateSound]?.PlayOnce();
            }
        }

        return false;
    }

    private void Activate(Vector2 position)
    {
        if (!_isActiveInput)
        {
            Activate();
        }
        
        var posX = position.X - ScreenBounds.X;
        if (AttachedView.Padding?.Left.HasValue ?? false)
        {
            var pad = AttachedView.Padding.Value.Left.Value.Calculate(CurrentScale, ScreenBounds.Width);
            posX -= pad;
        }
         
        var font = GetFont();
        
        _cursorPosition = _currentText.Length;
        
        for(var idx = 0; idx < _currentText.Length; idx++)
        {
            var size1 = font.TextSize(new TextSource(_currentText, 0, idx)).Width * TextScale;
            var size2 = font.TextSize(new TextSource(_currentText, 0, idx+1)).Width * TextScale;
            if (posX < (size1+size2) / 2f)
            {
                _cursorPosition = idx;
                break;
            }
        }
    }

    private void Activate()
    {
        if (_currentTextInput != null)
            return;
        
        _cursorPosition = _currentText.Length;
        _isActiveInput = true;

        var (rect,  cursorPos) = CalculateNativePosition();
        _currentTextInput = nativeTextInputService.AllocateTextInput(this, InputType.Text, rect, cursorPos);
    }

    private void UpdateNativePosition()
    {
        if (_currentTextInput is null)
            return;

        var (rect,  cursorPos) = CalculateNativePosition();
        _currentTextInput.UpdatePosition(rect, cursorPos);
    }

    private (RectangleF, int) CalculateNativePosition()
    {
        var textRect = GetTextRectangle();
        var sb = ScreenBounds;

        var frameWidth = AttachedView.ActiveFrameThickness?.Calculate(CurrentScale, 1) ?? CurrentScale + 1;
        frameWidth *= 2;
        
        var cpPixels = (int)Vector2.TransformNormal(new Vector2(_cursorPositionInPixels + textRect.X - sb.X + frameWidth, 0), inputCoordinateSystem.TransformInv).X;

        sb = sb.Inflate(frameWidth);
        
        var topLeft = Vector2.Transform(sb.TopLeft, inputCoordinateSystem.TransformInv);
        var bottomRight = Vector2.Transform(sb.BottomRight, inputCoordinateSystem.TransformInv);
        
        return (new RectangleF(topLeft, bottomRight - topLeft), cpPixels);
    }

    private void Deactivate()
    {
        var cti = _currentTextInput;
        _currentTextInput = null;
        cti?.Dispose();

        if (AttachedView.UpdateMode == TextUpdateMode.Unfocus)
        {
            AttachedView.Text?.SetText(_currentText);
        }
        else
        {
            _currentText = AttachedView.Text?.ToString() ?? "";
            _cursorPosition = _currentText?.Length ?? 0;
        }

        _isActiveInput = false;
    }

    public void SetFocus() => Focused = true;

    public bool ResetFocus()
    {
        Focused = false;
        Deactivate();
        return true;
    }

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
        _hovered = Enabled && hoverPosition.HasValue &&
                   (!matchingPointerId.HasValue || matchingPointerId.Value == _currentPointerId) &&
                   ScreenBounds.Contains(hoverPosition.Value);
    }

    public bool ProcessInput(IReadOnlyList<Pointer> pointers, IInputContext context)
    {
        if (_isActiveInput)
        {
            return ProcessActiveInput(pointers, context);
        }
        
        if(_wasActiveInput) return false;
        
        if (_currentPointerId.HasValue)
        {
            var pointer = pointers.FirstOrDefault(o => o.Id == _currentPointerId.Value);
            if (pointer is not null)
            {
                if (!pointer.State.IsDown)
                {
                    if (pointer.State.IsChanged && ScreenBounds.Contains(pointer.Position))
                    {
                        (uiSounds[UiSounds.ExecuteSound] ?? uiSounds[UiSounds.ButtonReleaseSound])?.PlayOnce();
                        Activate(pointer.Position);
                    }
                    _currentPointerId = null;
                    _pushed = false;
                }
                else
                {
                    var wasPressed = _pushed;
                    _pushed = ScreenBounds.Contains(pointer.Position);
                    if (wasPressed != _pushed)
                    {
                        if (_pushed) uiSounds[UiSounds.ButtonPushSound]?.PlayOnce();
                        else uiSounds[UiSounds.ButtonReleaseSound]?.PlayOnce();
                    }
                }
                return true;
            }
            _currentPointerId = null;
            _pushed = false;
        }

        foreach (var pointer in pointers)
        {
            if (pointer.State == ButtonState.JustPressed)
            {
                if (ScreenBounds.Contains(pointer.Position))
                {
                    _currentPointerId = pointer.Id;
                    _pushed = true;
                    uiSounds[UiSounds.ButtonPushSound]?.PlayOnce();

                    var focusable = context.FindFocusable(null, this);
                    if (focusable != null)
                    {
                        context.Focus(this, this);
                        pageInputContext.ShowFocus = false;
                    }
                    context.CapturePointer(pointer.Id, this);
                    return true;
                }

                Deactivate();
            }
        }

        return false;
    }

    private bool ProcessActiveInput(IReadOnlyList<Pointer> pointers, IInputContext context)
    {
        if (_currentPointerId != null)
        {
            var pointer = pointers.FirstOrDefault(o => o.Id == _currentPointerId.Value);
            if (pointer is not null)
            {
                if (!pointer.State.IsDown)
                {
                    _currentPointerId = null;
                    return true;
                }
                
                if (ScreenBounds.Contains(pointer.Position))
                {
                    Activate(pointer.Position);
                }
            }

            return true;
        }

        var ptr =
            pointers.FirstOrDefault(o => o.State == ButtonState.JustPressed && ScreenBounds.Contains(o.Position));

        if (ptr != null)
        {
            context.CapturePointer(ptr.Id, this);
            _currentPointerId = ptr.Id;
            Activate(ptr.Position);
            return true;
        }

        if (pointers.Any(o => o.State == ButtonState.JustPressed))
        {
            Deactivate();
        }
        return false;
    }

    public void CancelPointer(int pointerId, IInputContext context)
    {
        if (_currentPointerId == pointerId || context.FindFocusable(null, this) != this)
        {
            _currentPointerId = null;
            _pushed = false;
        }
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);

        _wasActiveInput = _isActiveInput;
        
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
                var frame = sb.Inflate(thickness + CurrentScale);

                renderer.GeometryRenderer.DrawFrame(frame, frameColor.Value, thickness);
            }
        }

        var font = GetFont();
        var textRect = GetTextRectangle();
        var isPlaceholder = string.IsNullOrEmpty(_currentText);

        if (isPlaceholder)
        {
            if (AttachedView.Placeholder is not null)
            {
                var placeholderColor = GetColor(AttachedView.PlaceholderColors, renderer);
                var placeholderOutlineColor = GetColor(AttachedView.PlaceholderOutlineColors, renderer);
                renderer.TextRenderer.DrawText(
                    font: font,
                    text: AttachedView.Placeholder,
                    position: textRect,
                    align: ContentAlign.Left | ContentAlign.VCenter,
                    scale: TextScale,
                    color: placeholderColor,
                    outlineColor: placeholderOutlineColor,
                    context: _textRenderingContext);
            }
        }
        else
        {
            var textColor = GetColor(AttachedView.TextColors, renderer);
            var textOutlineColor = GetColor(AttachedView.TextOutlineColors, renderer);
            renderer.TextRenderer.DrawText(
                font: font,
                text: _currentText,
                position: textRect,
                align: ContentAlign.Left | ContentAlign.VCenter,
                scale: TextScale,
                color: textColor,
                outlineColor: textOutlineColor,
                context: _textRenderingContext);
        }
        
        if (_isActiveInput)
        {
            var positionX = font.TextSize(new TextSource(_currentText, 0, _cursorPosition)).Width * TextScale;
            

            if (Math.Abs(positionX - _cursorPositionInPixels) > 0.001f)
            {
                _cursorPositionInPixels = positionX;
                UpdateNativePosition();
            }
            
            if (DateTime.Now.TimeOfDay.TotalSeconds % 1 < 0.5f)
            {
                var textOutlineColor = GetColor(AttachedView.TextOutlineColors, renderer);
                frameColor = AttachedView.CursorColor?.GetColor(paletteSource, renderer) ?? frameColor;
                
                var cursorRect = new RectangleF(textRect.X + positionX - 0.5f, textRect.Y, TextScale,
                    font.LineSize * TextScale);

                renderer.GeometryRenderer.FillRectangle(cursorRect, frameColor ?? RgbaColor.White);

                cursorRect = cursorRect.Inflate(TextScale);

                renderer.GeometryRenderer.DrawFrame(cursorRect, textOutlineColor ?? RgbaColor.White, TextScale);
            }
        }
    }

    private RectangleF GetTextRectangle()
    {
        var xx = ScreenBounds.X;
        var yy = ScreenBounds.Y;
        var width = ScreenBounds.Width;
        var height = ScreenBounds.Height;

        if (AttachedView.Padding?.Left.HasValue ?? false)
        {
            var pad = AttachedView.Padding.Value.Left.Value.Calculate(CurrentScale, ScreenBounds.Width);
            width -= pad;
            xx += pad;
        }
        if (AttachedView.Padding?.Right.HasValue ?? false)
        {
            width -= AttachedView.Padding.Value.Right.Value.Calculate(CurrentScale, ScreenBounds.Width);
        }
        if (AttachedView.Padding?.Top.HasValue ?? false)
        {
            var pad = AttachedView.Padding.Value.Top.Value.Calculate(CurrentScale, ScreenBounds.Height);
            height -= pad;
            yy += pad;
        }
        if (AttachedView.Padding?.Bottom.HasValue ?? false)
        {
            height -= AttachedView.Padding.Value.Bottom.Value.Calculate(CurrentScale, ScreenBounds.Height);
        }

        return new RectangleF(xx, yy, width, height);
    }

    private RgbaColor? GetColor(IButtonStateColors colors, IRenderer2 renderer) => colors?.GetColor(renderer,
        paletteSource, _hovered, Focused, _pushed, Enabled, _isActiveInput);

    public void OnTextInput(string text)
    {
        var beforeCursor = _currentText.Substring(0, _cursorPosition);
        var afterCursor = _currentText.Substring(_cursorPosition);
        
        _currentText = beforeCursor + text + afterCursor;
        _cursorPosition += text.Length;
        
        if (AttachedView.UpdateMode == TextUpdateMode.Live)
        {
            AttachedView.Text?.SetText(_currentText);
        }
    }

    public void OnTextInputClosed() => Deactivate();
    
    public bool OnKey(Key key)
    {
        switch(key)
        {
            case Key.Enter:
                if (_isActiveInput)
                {
                    AttachedView.Text?.SetText(_currentText);
                    Deactivate();
                    return true;
                }
                break;
            
            case Key.Escape:
                if (_isActiveInput)
                {
                    Deactivate();
                    return true;
                }
                break;
            case Key.Backspace:
                if (_cursorPosition > 0)
                {
                    _currentText = _currentText.Substring(0, _cursorPosition - 1) + _currentText.Substring(_cursorPosition);
                    _cursorPosition--;
                    
                    if (AttachedView.UpdateMode == TextUpdateMode.Live)
                    {
                        AttachedView.Text?.SetText(_currentText);
                    }
                }
                return true;
            
            case Key.Delete:
                if (_currentText.Length > _cursorPosition)
                {
                    _currentText = _currentText.Substring(0, _cursorPosition) +
                                   _currentText.Substring(_cursorPosition + 1);
                    
                    if (AttachedView.UpdateMode == TextUpdateMode.Live)
                    {
                        AttachedView.Text?.SetText(_currentText);
                    }
                }
                return true;
            
            case Key.Left:
                _cursorPosition = Math.Max(0, _cursorPosition - 1);
                return true;
            
            case Key.Right:
                _cursorPosition = Math.Min(_currentText.Length, _cursorPosition + 1);
                return true;
        }
        return false;
    }
}
