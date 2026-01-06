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
    private readonly IUiSounds _uiSounds;
    private readonly IHapticDevice _hapticDevice;
    private TView AttachedView => (TView)_viewHandler.View;
    
    private int? _currentPointerId;

    private bool _isExecutingDelayedCommand;
    private bool _isEnabled;

    private bool HapticEnabled => (_viewHandler?.View as IButtonView)?.HapticFeedback?.Value ?? false;
    
    public ButtonHelper(TViewHandler viewHandler, IUiSounds uiSounds, IHapticDevice hapticDevice)
    {
        _viewHandler = viewHandler;
        _uiSounds = uiSounds;
        _hapticDevice = hapticDevice;

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
                            if (HapticEnabled)
                            {
                                _hapticDevice.Feedback(FeedbackStyle.Release, pointer.OriginalPosition);
                            }

                            _uiSounds[UiSounds.ExecuteSound]?.PlayOnce();
                            Execute(TimeSpan.Zero, true == AttachedView?.EnableCommandType ? ButtonCommandType.Select : null);
                            _currentPointerId = null;
                            return true;
                        }
                    }
                    _currentPointerId = null;
                    IsPressed = false;
                }
                else
                {
                    var wasPressed = IsPressed;
                    IsPressed = _viewHandler.ScreenBounds.Contains(pointer.Position);

                    if (wasPressed != IsPressed && HapticEnabled)
                    {
                        if (IsPressed)
                        {
                            _hapticDevice.Feedback(FeedbackStyle.Push, pointer.OriginalPosition);
                        }
                        else
                        {
                            _hapticDevice.Feedback(FeedbackStyle.Release, pointer.OriginalPosition);
                        }
                    }
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

                    if (HapticEnabled)
                    {
                        _hapticDevice.Feedback(FeedbackStyle.Push, pointer.OriginalPosition);
                    }

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
                
                if (focusId is null && true == AttachedView?.EnableCommandType)
                {
                    Execute(AttachedView.KeyCommandDelay, ButtonCommandType.Previous);
                }
                break;
            
            case UiButton.Right:
                focusId = AttachedView?.HorizontalNavigation.right;
                
                if (focusId is null && true == AttachedView?.EnableCommandType)
                {
                    Execute(AttachedView.KeyCommandDelay, ButtonCommandType.Next);
                }
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
                    _uiSounds[UiSounds.ExecuteSound]?.PlayOnce();
                    Execute(AttachedView.KeyCommandDelay, true == AttachedView?.EnableCommandType ? ButtonCommandType.Select : null);
                }
                break;
        }

        if (focusId != null)
        {
            var focusable = context.FindFocusable(focusId, _viewHandler);
            if (focusable != null)
            {
                if (!focusable.SkipNavigation && focusable.Enabled)
                {
                    context.Focus(focusable, _viewHandler);
                    _uiSounds[UiSounds.ItemNavigateSound]?.PlayOnce();
                    return true;
                }
                
                return focusable.OnUiButton(button, context);
            }
        }

        return false;
    }

    private void Execute(TimeSpan delay, ButtonCommandType? commandType)
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

            if (commandType.HasValue)
            {
                await asyncCommand.ExecuteAsync((AttachedView.CommandParameter, commandType.Value));
            }
            else
            {
                await asyncCommand.ExecuteAsync(AttachedView.CommandParameter);
            }

        }).ContinueWith(_ =>
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