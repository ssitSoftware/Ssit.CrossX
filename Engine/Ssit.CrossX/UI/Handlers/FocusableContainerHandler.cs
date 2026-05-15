using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class FocusableContainerHandler(ViewHandler.CreateHandlerParameters parameters, IHandlerMapper handlerMapper, PageInputContext pageInputContext, IUiSounds uiSounds, IRenderer2 renderer, IPaletteSource paletteSource = null) 
    : ContainerHandler<FocusableContainer>(parameters, handlerMapper, paletteSource), IFocusable,
        IInputConsumer, IColorSource
{
    protected override RgbaColor? BackgroundColor(IRenderer2 _) => Focused ? AttachedView.FocusBackgroundColor?.GetColor(PaletteSource, renderer) ?? base.BackgroundColor(renderer) : base.BackgroundColor(renderer);
    
    public bool Enabled => !string.IsNullOrWhiteSpace(AttachedView.UniqueId);
    public bool Focused { get; private set; }
    public bool DisableAllInput => false;
    
    public RgbaColor? GetColor(string id)
    {
        if (Focused)
        {
            switch (id)
            {
                case nameof(FocusableContainer.FocusColor):
                    return AttachedView.FocusColor?.GetColor(paletteSource, renderer);
                
                case nameof(FocusableContainer.FocusOutlineColor):
                    return AttachedView.FocusOutlineColor?.GetColor(paletteSource, renderer);
            }
        }

        return null;
    }
    
    public bool OnUiButton(UiButton button, IInputContext context)
    {
        foreach (var child in Children)
        {
            if (child is IUiCommandHandler handler)
            {
                if (handler.OnUiButton(button, context))
                    return true;
            }
        }
        
        var focusDirection = button switch
        {
            UiButton.Up => FocusDirection.Up,
            UiButton.Down => FocusDirection.Down,
            UiButton.Left => FocusDirection.Left,
            UiButton.Right => FocusDirection.Right,
            _ => FocusDirection.None
        };

        if (focusDirection == FocusDirection.None) 
            return false;
        
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

        return false;
    }

    public void SetFocus() => Focused = true;

    public bool ResetFocus()
    {
        Focused = false;
        return true;
    }

    public string UniqueId => AttachedView?.UniqueId;
    public bool SkipNavigation => false;
    
    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context)
    {
    }

    public bool ProcessInput(IReadOnlyList<Pointer> pointers, IInputContext context)
    {
        return false;
    }

    public void CancelPointer(int pointerId, IInputContext context)
    {
    }
}