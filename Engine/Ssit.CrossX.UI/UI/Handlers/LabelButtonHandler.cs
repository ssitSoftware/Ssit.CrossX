using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelButtonHandler<TLabelButton>: LabelHandler<TLabelButton>, IInputConsumer, IFocusable where TLabelButton: LabelButton
{
    protected override RgbaColor? BackgroundColor => Enabled ? _isFocused ? AttachedView.FocusedBackgroundColor ?? AttachedView.HoverBackgroundColor ?? AttachedView.BackgroundColor 
        : _isHovered ? AttachedView.HoverBackgroundColor ?? AttachedView.BackgroundColor : AttachedView.BackgroundColor : AttachedView.DisabledBackgroundColor ?? AttachedView.BackgroundColor;
    
    protected override RgbaColor? TextColor => Enabled ? _isFocused ? AttachedView.FocusedTextColor ?? AttachedView.HoverTextColor ?? AttachedView.TextColor 
        : _isHovered ? AttachedView.HoverTextColor ?? AttachedView.TextColor : AttachedView.TextColor : AttachedView.DisabledTextColor ?? AttachedView.TextColor;
    protected override RgbaColor? TextOutlineColor => Enabled ? _isFocused ? AttachedView.FocusedTextOutlineColor ?? AttachedView.HoverTextOutlineColor ?? AttachedView.TextOutlineColor 
        : _isHovered ? AttachedView.HoverTextOutlineColor ?? AttachedView.TextOutlineColor : AttachedView.TextOutlineColor : AttachedView.DisabledTextOutlineColor ?? AttachedView.TextOutlineColor;

    private bool _isFocused;
    private bool _isHovered;
    
    public bool Enabled { get; private set; }
    
    public LabelButtonHandler(CreateHandlerParameters parameters, IFontsManager fontsManager) : base(parameters, fontsManager)
    {
        Enabled = true;
        if (AttachedView.Command is not null)
        {
            AttachedView.Command.CanExecuteChanged += CommandOnCanExecuteChanged;
            Enabled = AttachedView.Command.CanExecute(AttachedView.CommandParameter);
        }
    }

    private void CommandOnCanExecuteChanged(object sender, EventArgs e)
    {
        Enabled = AttachedView.Command.CanExecute(AttachedView.CommandParameter);
    }

    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context)
    {
        _isHovered = Enabled && hoverPosition.HasValue && !matchingPointerId.HasValue && ScreenBounds.Contains(hoverPosition.Value);
    }

    public bool ProcessInput(IReadOnlyList<Pointer> pointer, IInputContext context)
    {
        return false;
    }

    public void CancelPointer(int pointerId, IInputContext context)
    {
    }

    public bool OnUiButton(UiButton button, IInputContext context)
    {
        string focusId = null;
        switch (button)
        {
            case UiButton.Left:
                focusId = AttachedView?.HorizontalNavigation.left;
                break;
            
            case UiButton.Right:
                focusId = AttachedView?.HorizontalNavigation.right;
                break;
            
            case UiButton.Up:
                focusId = AttachedView?.VerticalNavigation.up;
                break;
            
            case UiButton.Down:
                focusId = AttachedView?.VerticalNavigation.down;
                break;
            
            case UiButton.Select:
                if (Enabled)
                {
                    AttachedView?.Command?.Execute(AttachedView?.CommandParameter);
                }
                break;
        }

        if (focusId != null)
        {
            var focusable = context.FindFocusable(focusId, this);
            if (focusable != null)
            {
                if (focusable.Enabled)
                {
                    context.Focus(focusable, this);
                    return true;
                }
                
                return focusable.OnUiButton(button, context);
            }
        }

        return false;
    }

    public void SetFocus()
    {
        _isFocused = true;
    }

    public bool ResetFocus()
    {
        _isFocused = false;
        return true;
    }

    public string UniqueId => AttachedView?.UniqueId;

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        
        if (AttachedView.Command is not null)
        {
            AttachedView.Command.CanExecuteChanged -= CommandOnCanExecuteChanged;
        }
    }
}