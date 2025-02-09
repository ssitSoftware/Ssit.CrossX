using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Handlers.Helpers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelButtonHandler<TLabelButton>: LabelHandler<TLabelButton>, IInputConsumer, IFocusable where TLabelButton: LabelButton
{
    protected override RgbaColor? BackgroundColor(RenderMode mode) => AttachedView.BackgroundColors?.GetColor(mode, _buttonHelper.IsHovered, Focused, _buttonHelper.IsPressed || _buttonHelper.IsExecutingCommand, Enabled );
    protected override RgbaColor? TextColor(RenderMode mode) => AttachedView.TextColors?.GetColor(mode,_buttonHelper.IsHovered, Focused, _buttonHelper.IsPressed || _buttonHelper.IsExecutingCommand, Enabled );
    protected override RgbaColor? TextOutlineColor(RenderMode mode) => AttachedView.TextOutlineColors?.GetColor(mode,_buttonHelper.IsHovered, Focused, _buttonHelper.IsPressed || _buttonHelper.IsExecutingCommand, Enabled );
    
    public bool Enabled => _buttonHelper.IsEnabled;
    public bool DisableAllInput => _buttonHelper.IsExecutingCommand;
    
    public bool Focused { get; private set; }

    public bool SkipNavigation => false;

    private readonly ButtonHelper<TLabelButton, LabelButtonHandler<TLabelButton>> _buttonHelper;

    public LabelButtonHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IActionDispatcher actionDispatcher, IUiSounds uiSounds) : base(parameters, fontsManager, actionDispatcher)
    {
        _buttonHelper = new ButtonHelper<TLabelButton, LabelButtonHandler<TLabelButton>>(this, uiSounds);
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