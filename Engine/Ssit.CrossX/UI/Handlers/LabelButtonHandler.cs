using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Handlers.Helpers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelButtonHandler<TLabelButton>: LabelHandler<TLabelButton>, IInputConsumer, IFocusable where TLabelButton: LabelButton
{
    private readonly PageInputContext _pageInputContext;
    protected override RgbaColor? BackgroundColor(IRenderer2 renderer) => AttachedView.BackgroundColors?.GetColor(renderer, PaletteSource, _buttonHelper.IsHovered, Focused && _pageInputContext.ShowFocus, _buttonHelper.IsPressed || _buttonHelper.IsExecutingCommand, Enabled, IsChecked);
    protected override RgbaColor? TextColor(IRenderer2 renderer, bool? focused = null) => AttachedView.TextColors?.GetColor(renderer, PaletteSource, _buttonHelper.IsHovered, (focused ?? Focused) && _pageInputContext.ShowFocus, _buttonHelper.IsPressed || _buttonHelper.IsExecutingCommand, Enabled, IsChecked);
    protected override RgbaColor? TextOutlineColor(IRenderer2 renderer) => AttachedView.TextOutlineColors?.GetColor(renderer, PaletteSource, _buttonHelper.IsHovered, Focused && _pageInputContext.ShowFocus, _buttonHelper.IsPressed || _buttonHelper.IsExecutingCommand, Enabled, IsChecked);
 
    protected virtual bool IsChecked => false;
    
    public bool Enabled => _buttonHelper.IsEnabled;
    public bool DisableAllInput => _buttonHelper.IsExecutingCommand;
    
    public bool Focused { get; private set; }
    
    public bool IsPushed => _buttonHelper.IsPressed;

    public bool SkipNavigation => false;

    private readonly ButtonHelper<TLabelButton, LabelButtonHandler<TLabelButton>> _buttonHelper;

    public LabelButtonHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, 
        IActionDispatcher actionDispatcher, IUiSounds uiSounds, IHapticDevice hapticDevice, 
        PageInputContext pageInputContext,
        IPaletteSource paletteSource = null) 
        : base(parameters, fontsManager, actionDispatcher, paletteSource)
    {
        _pageInputContext = pageInputContext;
        _buttonHelper = new ButtonHelper<TLabelButton, LabelButtonHandler<TLabelButton>>(this, AttachedView?.CustomSounds ?? uiSounds, hapticDevice, pageInputContext);
    }

    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context) => _buttonHelper.ProcessHover(hoverPosition, matchingPointerId, context);

    public bool ProcessInput(IReadOnlyList<Pointer> pointer, IInputContext context) => _buttonHelper.ProcessInput(pointer, context);

    public void CancelPointer(int pointerId, IInputContext context) => _buttonHelper.CancelPointer(pointerId, context);

    public bool OnUiButton(UiButton button, IInputContext context)
    {
        try
        {
            return _buttonHelper.OnUiButton(button, context);
        }
        catch (Exception ex)
        {
            Debugger.Break();
        }

        return false;
    }

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