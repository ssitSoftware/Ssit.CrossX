using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Handlers.Helpers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelButtonHandler<TLabelButton>: LabelHandler<TLabelButton>, IInputConsumer, IFocusable where TLabelButton: LabelButton
{
    protected override RgbaColor? BackgroundColor => Enabled ? Focused ? AttachedView.FocusedBackgroundColor ?? AttachedView.HoverBackgroundColor ?? AttachedView.BackgroundColor 
        : _buttonHelper.IsHovered ? AttachedView.HoverBackgroundColor ?? AttachedView.BackgroundColor : AttachedView.BackgroundColor : AttachedView.DisabledBackgroundColor ?? AttachedView.BackgroundColor;
    
    protected override RgbaColor? TextColor => Enabled ? Focused ? AttachedView.FocusedTextColor ?? AttachedView.HoverTextColor ?? AttachedView.TextColor 
        : _buttonHelper.IsHovered ? AttachedView.HoverTextColor ?? AttachedView.TextColor : AttachedView.TextColor : AttachedView.DisabledTextColor ?? AttachedView.TextColor;
    protected override RgbaColor? TextOutlineColor => Enabled ? Focused ? AttachedView.FocusedTextOutlineColor ?? AttachedView.HoverTextOutlineColor ?? AttachedView.TextOutlineColor 
        : _buttonHelper.IsHovered ? AttachedView.HoverTextOutlineColor ?? AttachedView.TextOutlineColor : AttachedView.TextOutlineColor : AttachedView.DisabledTextOutlineColor ?? AttachedView.TextOutlineColor;
    
    
    public bool Enabled => _buttonHelper.IsEnabled;
    public bool Focused { get; private set; }

    public bool SkipNavigation => false;

    private readonly ButtonHelper<TLabelButton, LabelButtonHandler<TLabelButton>> _buttonHelper;

    public LabelButtonHandler(CreateHandlerParameters parameters, IFontsManager fontsManager) : base(parameters, fontsManager)
    {
        _buttonHelper = new ButtonHelper<TLabelButton, LabelButtonHandler<TLabelButton>>(this);
    }

    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context) => _buttonHelper.ProcessHover(hoverPosition, matchingPointerId, context);

    public bool ProcessInput(IReadOnlyList<Pointer> pointer, IInputContext context) => _buttonHelper.ProcessInput(pointer, context);

    public void CancelPointer(int pointerId, IInputContext context) => _buttonHelper.CancelPointer(pointerId, context);

    public bool OnUiButton(UiButton button, IInputContext context) => _buttonHelper.OnUiButton(button, context);

    public void SetFocus()
    {
        Focused = true;
    }

    public bool ResetFocus()
    {
        Focused = false;
        return true;
    }

    public string UniqueId => AttachedView?.UniqueId;

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _buttonHelper.Dispose();
    }
}