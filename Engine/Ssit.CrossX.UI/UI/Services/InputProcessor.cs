using System;
using System.Collections.Generic;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Services;

internal sealed class InputProcessor: IInputContext
{
    private readonly IKeyboard _keyboard;
    private readonly IGameControllers _gameControllers;
    private readonly IPointingDevices _pointingDevices;
    private readonly Navigation _navigation;

    private readonly List<IInputConsumer> _inputConsumers = new();
    private readonly List<(int, IInputConsumer)> _capturedPointers = new();
    
    public InputProcessor(IKeyboard keyboard, IGameControllers gameControllers, IPointingDevices pointingDevices, Navigation navigation)
    {
        _keyboard = keyboard;
        _gameControllers = gameControllers;
        _pointingDevices = pointingDevices;
        _navigation = navigation;
    }

    private void PrepareInputConsumers(ViewHandler handler)
    {
        if (handler is IInputConsumer consumer)
        {
            _inputConsumers.Add(consumer);
        }
        
        if (handler is IChildrenContainer container)
        {
            foreach (var child in container.Children)
            {
                PrepareInputConsumers(child);
            }
        }
    }

    private void OnUiButton(IPage page, UiButton button)
    {
        if (true == page.FocusedElement?.DisableAllInput)
            return;
        
        if (page.OnUiButton(button, this))
        {
            return;
        }
        
        page.FocusedElement?.OnUiButton(button, this);
    }
    
    public void Process()
    {
        var page = _navigation.CurrentPage;
        
        if (GetUiButtonLeft())
        {
            OnUiButton(page, UiButton.Left);
            if (page != _navigation.CurrentPage) return;
        }

        if (GetUiButtonRight())
        {
            OnUiButton(page, UiButton.Right);
            if (page != _navigation.CurrentPage) return;
        }

        if (GetUiButtonUp())
        {
            OnUiButton(page, UiButton.Up);
            if (page != _navigation.CurrentPage) return;
        }

        if (GetUiButtonDown())
        {
            OnUiButton(page, UiButton.Down);
            if (page != _navigation.CurrentPage) return;
        }

        if (GetUiButtonSelect())
        {
            OnUiButton(page, UiButton.Select);
            if (page != _navigation.CurrentPage) return;
        }
        
        if (GetUiButtonMenu())
        {
            OnUiButton(page, UiButton.Menu);
            if (page != _navigation.CurrentPage) return;
        }

        if (GetUiButtonMenuOrBack())
        {
            OnUiButton(page, UiButton.MenuOrBack);
            if (page != _navigation.CurrentPage) return;
        }

        if (GetUiButtonBack())
        {
            OnUiButton(page, UiButton.Back);
            if (page != _navigation.CurrentPage) return;
        }
        
        _inputConsumers.Clear();
        PrepareInputConsumers(page.RootHandler);

        int? matchingPointerId = null;
        foreach (var ptr in _pointingDevices.Pointers)
        {
            if (ptr.Position == _pointingDevices.HoverPosition)
            {
                matchingPointerId = ptr.Id;
                break;
            }
        }

        bool disableInput = true == page.FocusedElement?.DisableAllInput;
        
        if (disableInput)
        {
            for (var idx = 0; idx < _pointingDevices.Pointers.Count; ++idx)
            {
                CapturePointer(_pointingDevices.Pointers[idx].Id, page.FocusedElement as IInputConsumer);
            }
            ProcessCapturedPointers();
        }
        else
        {
            for (var idx = _inputConsumers.Count - 1; idx >= 0; --idx)
            {
                _inputConsumers[idx].ProcessHover(_pointingDevices.HoverPosition, matchingPointerId, this);
            }

            for (var idx = _inputConsumers.Count - 1; idx >= 0; --idx)
            {
                if (_inputConsumers[idx].ProcessInput(_pointingDevices.Pointers, this))
                {
                    ProcessCapturedPointers();
                    _inputConsumers[idx].ProcessHover(_pointingDevices.HoverPosition, matchingPointerId, this);
                    break;
                }
            }
        }
    }

    private void ProcessCapturedPointers()
    {
        if (_capturedPointers.Count > 0)
        {
            foreach (var (pointerId, consumer) in _capturedPointers)
            {
                foreach (var cons in _inputConsumers)
                {
                    if (cons != consumer)
                    {
                        cons.CancelPointer(pointerId, this);
                    }
                }
            }
            _capturedPointers.Clear();
        }
    }
    
    private bool GetUiButtonSelect()
    {
        return _keyboard.GetKey(Key.Return) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.A) == ButtonState.JustPressed;
    }
    
    private bool GetUiButtonBack()
    {
        return _gameControllers.GetButton(0, GameControllerButton.B) == ButtonState.JustPressed;
    }
    
    private bool GetUiButtonMenu()
    {
        return _gameControllers.GetButton(0, GameControllerButton.B) == ButtonState.JustPressed;
    }

    private bool GetUiButtonMenuOrBack()
    {
        return _keyboard.GetKey(Key.Escape) == ButtonState.JustPressed;
    }
    
    private bool GetUiButtonLeft()
    {
        return _keyboard.GetKey(Key.Left) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.DPadLeft) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.LeftStickLeft) == ButtonState.JustPressed;
    }
    
    private bool GetUiButtonRight()
    {
        return _keyboard.GetKey(Key.Right) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.DPadRight) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.LeftStickRight) == ButtonState.JustPressed;

    }
    
    private bool GetUiButtonUp()
    {
        return _keyboard.GetKey(Key.Up) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.DPadUp) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.LeftStickUp) == ButtonState.JustPressed;
    }
    
    private bool GetUiButtonDown()
    {
        return _keyboard.GetKey(Key.Down) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.DPadDown) == ButtonState.JustPressed ||
               _gameControllers.GetButton(0, GameControllerButton.LeftStickDown) == ButtonState.JustPressed;

    }

    public void CapturePointer(int pointerId, IInputConsumer captureBy)
    {
        _capturedPointers.Add((pointerId, captureBy));
    }

    public bool Focus(IFocusable focusable, object caller)
    {
        var page = GetPage(caller);
        if (false == page?.FocusedElement?.ResetFocus())
        {
            return false;
        }
        
        focusable?.SetFocus();
        if (page != null)
        {
            page.FocusedElement = focusable;
        }
        return true;
    }

    private IPage GetPage(object caller)
    {
        if ( caller is IPage page)
        {
            return page;
        }

        if (caller is ViewHandler vh)
        {
            caller = vh.Parent;
        }
        
        if (caller is IViewParent parent)
        {
            while (parent is not null)
            {
                if (parent is IPage page1)
                {
                    return page1;
                }
                
                if (parent is ViewHandler handler)
                {
                    parent = handler.Parent;
                }
                else throw new InvalidProgramException();
            }
        }
        throw new InvalidProgramException();
    }

    public IFocusable FindFocusable(string uniqueId, object caller)
    {
        var page = GetPage(caller);

        if (uniqueId == null) return page.FocusedElement;
        return FindFocusable(page.RootHandler, uniqueId);
    }
    
    private IFocusable FindFocusable(ViewHandler handler, string name)
    {
        if (handler is IFocusable focusable && focusable.UniqueId == name)
        {
            return focusable;
        }
        
        if (handler is IChildrenContainer container)
        {
            foreach (var child in container.Children)
            {
                var ret = FindFocusable(child, name);
                if(ret != null) return ret;
            }
        }

        return null;
    }
}