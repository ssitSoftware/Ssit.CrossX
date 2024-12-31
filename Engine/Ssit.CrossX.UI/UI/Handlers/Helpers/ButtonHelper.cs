using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers.Helpers;

public class ButtonHelper<TView, TViewHandler>: IDisposable where TView: View, IButtonView where TViewHandler: ViewHandler<TView>, IFocusable, IInputConsumer
{
    public bool IsEnabled
    {
        get => _isEnabled || _isExecutingDelayedCommand;
        private set => _isEnabled = value;
    }

    public bool IsHovered { get; private set; }
    public bool IsPressed { get; private set; }
    public bool IsExecutingCommand => _isExecutingDelayedCommand;

    private readonly TViewHandler _viewHandler;
    private TView AttachedView => (TView)_viewHandler.View;
    
    private int? _currentPointerId;

    private bool _isExecutingDelayedCommand = false;
    private bool _isEnabled;

    public ButtonHelper(TViewHandler viewHandler)
    {
        _viewHandler = viewHandler;
        
        if (AttachedView.Command is not null)
        {
            AttachedView.Command.CanExecuteChanged += CommandOnCanExecuteChanged;
            IsEnabled = AttachedView.Command.CanExecute(AttachedView.CommandParameter);
        }
    }
    
    private void CommandOnCanExecuteChanged(object sender, EventArgs e)
    {
        IsEnabled = AttachedView.Command.CanExecute(AttachedView.CommandParameter);
    }
    
    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context)
    {
        if (_isExecutingDelayedCommand)
        {
            return;
        }
        
        IsHovered = !_isExecutingDelayedCommand && IsEnabled && hoverPosition.HasValue && 
                    (!matchingPointerId.HasValue || matchingPointerId.Value == _currentPointerId)  &&
                    _viewHandler.ScreenBounds.Contains(hoverPosition.Value);
    }

    public bool ProcessInput(IReadOnlyList<Pointer> pointers, IInputContext context)
    {
        if (_isExecutingDelayedCommand)
        {
            return true;
        }

        if ( _currentPointerId.HasValue )
        {
            var pointer = pointers.FirstOrDefault(o => o.Id == _currentPointerId.Value);
            if (pointer is not null)
            {
                if (!pointer.State.IsDown)
                {
                    if (pointer.State.IsChanged)
                    {
                        if (_viewHandler.ScreenBounds.Contains(pointer.Position))
                        {
                            Execute(TimeSpan.Zero);
                            _currentPointerId = null;
                            return true;
                        }
                    }
                    _currentPointerId = null;
                    IsPressed = false;
                }
                else
                {
                    IsPressed = _viewHandler.ScreenBounds.Contains(pointer.Position);
                }
                return true;
            }
            _currentPointerId = null;
            IsPressed = false;
        }
        
        foreach (var pointer in pointers)
        {
            if (pointer.State == ButtonState.JustPressed)
            {
                if (_viewHandler.ScreenBounds.Contains(pointer.Position))
                {
                    _currentPointerId = pointer.Id;
                    IsPressed = true;
                    
                    context.CapturePointer(pointer.Id, _viewHandler);
                    var focusable = context.FindFocusable(null, _viewHandler);
                    if (focusable != null)
                    {
                        context.Focus(_viewHandler, _viewHandler);
                    }
                    return true;
                }
            }
        }
        
        return false;
    }

    public void CancelPointer(int pointerId, IInputContext context)
    {
        if (_currentPointerId == pointerId)
        {
            _currentPointerId = null;
            IsPressed = false;
        }
    }

    public bool OnUiButton(UiButton button, IInputContext context)
    {
        if (_isExecutingDelayedCommand)
        {
            return true;
        }
        
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
                if (IsEnabled)
                {
                    Execute(AttachedView.KeyCommandDelay);
                }
                break;
        }

        if (focusId != null)
        {
            var focusable = context.FindFocusable(focusId, _viewHandler);
            if (focusable != null)
            {
                if (!focusable.SkipNavigation)
                {
                    context.Focus(focusable, _viewHandler);
                    return true;
                }
                
                return focusable.OnUiButton(button, context);
            }
        }

        return false;
    }

    private void Execute(TimeSpan delay)
    {
        if (AttachedView.Command is not IAsyncCommand asyncCommand)
        {
            asyncCommand = new AsyncCommand( o => Task.Run( async () =>
            {
                await Task.Yield();
                AttachedView?.Command?.Execute(o);
            }));
        }
        
        IsPressed = true;
        _isExecutingDelayedCommand = true;

        Task.Run(async () =>
        {
            if (delay.TotalSeconds > 0)
            {
                await Task.Delay(delay);
            }

            IsPressed = false;
            if (AttachedView.CommandDelay.TotalSeconds > 0)
            {
                await Task.Delay(AttachedView.CommandDelay);
            }

            await asyncCommand.ExecuteAsync(AttachedView.CommandParameter);
        }).ContinueWith(t =>
        {
            IsPressed = false;
            _isExecutingDelayedCommand = false;
        });
    }

    public void Dispose()
    {
        if (AttachedView.Command is not null)
        {
            AttachedView.Command.CanExecuteChanged -= CommandOnCanExecuteChanged;
        }
    }
}