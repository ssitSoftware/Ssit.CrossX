#if __IOS__ || __MACCATALYST__

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Foundation;
using MetalKit;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Ssit.CrossX.Core;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.NET.Apple.Input;
using Ssit.CrossX.NET.Input;
using UIKit;

namespace Ssit.CrossX.NET.Apple;

internal sealed class PixelViewController : UIViewController
{
    private readonly MTKView _metalView;

    private static readonly Dictionary<UIKeyboardHidUsage, Key> KeyMapping = new()
    {
        {UIKeyboardHidUsage.Keyboard0, Key.D0},
        {UIKeyboardHidUsage.Keyboard1, Key.D1},
        {UIKeyboardHidUsage.Keyboard2, Key.D2},
        {UIKeyboardHidUsage.Keyboard3, Key.D3},
        {UIKeyboardHidUsage.Keyboard4, Key.D4},
        {UIKeyboardHidUsage.Keyboard5, Key.D5},
        {UIKeyboardHidUsage.Keyboard6, Key.D6},
        {UIKeyboardHidUsage.Keyboard7, Key.D7},
        {UIKeyboardHidUsage.Keyboard8, Key.D8},
        {UIKeyboardHidUsage.Keyboard9, Key.D9},
        {UIKeyboardHidUsage.KeyboardA, Key.A},
        {UIKeyboardHidUsage.KeyboardB, Key.B},
        {UIKeyboardHidUsage.KeyboardC, Key.C},
        {UIKeyboardHidUsage.KeyboardD, Key.D},
        {UIKeyboardHidUsage.KeyboardE, Key.E},
        {UIKeyboardHidUsage.KeyboardF, Key.F},
        {UIKeyboardHidUsage.KeyboardG, Key.G},
        {UIKeyboardHidUsage.KeyboardH, Key.H},
        {UIKeyboardHidUsage.KeyboardI, Key.I},
        {UIKeyboardHidUsage.KeyboardJ, Key.J},
        {UIKeyboardHidUsage.KeyboardK, Key.K},
        {UIKeyboardHidUsage.KeyboardL, Key.L},
        {UIKeyboardHidUsage.KeyboardM, Key.M},
        {UIKeyboardHidUsage.KeyboardN, Key.N},
        {UIKeyboardHidUsage.KeyboardO, Key.O},
        {UIKeyboardHidUsage.KeyboardP, Key.P},
        {UIKeyboardHidUsage.KeyboardQ, Key.Q},
        {UIKeyboardHidUsage.KeyboardR, Key.R},
        {UIKeyboardHidUsage.KeyboardS, Key.S},
        {UIKeyboardHidUsage.KeyboardT, Key.T},
        {UIKeyboardHidUsage.KeyboardU, Key.U},
        {UIKeyboardHidUsage.KeyboardV, Key.V},
        {UIKeyboardHidUsage.KeyboardW, Key.W},
        {UIKeyboardHidUsage.KeyboardX, Key.X},
        {UIKeyboardHidUsage.KeyboardY, Key.Y},
        {UIKeyboardHidUsage.KeyboardZ, Key.Z},
        {UIKeyboardHidUsage.KeyboardLeftArrow, Key.Left},
        {UIKeyboardHidUsage.KeyboardRightArrow, Key.Right},
        {UIKeyboardHidUsage.KeyboardUpArrow, Key.Up},
        {UIKeyboardHidUsage.KeyboardDownArrow, Key.Down},
        {UIKeyboardHidUsage.KeyboardEscape, Key.Escape},
        {UIKeyboardHidUsage.KeyboardReturn, Key.Return},
        {UIKeyboardHidUsage.KeyboardReturnOrEnter, Key.Return},
        {UIKeyboardHidUsage.KeyboardExecute, Key.Return},
        {UIKeyboardHidUsage.KeyboardSpacebar, Key.Space},
        {UIKeyboardHidUsage.KeyboardDeleteOrBackspace, Key.Backspace},
        {UIKeyboardHidUsage.KeyboardSlash, Key.Slash},
        {UIKeyboardHidUsage.KeyboardBackslash, Key.Backslash},
        {UIKeyboardHidUsage.KeyboardNonUSBackslash, Key.Backslash},
        {UIKeyboardHidUsage.KeyboardTab, Key.Tab},
        {UIKeyboardHidUsage.KeyboardHome, Key.Home},
        {UIKeyboardHidUsage.KeyboardEnd, Key.End},
        {UIKeyboardHidUsage.KeyboardInsert, Key.Insert},
        {UIKeyboardHidUsage.KeyboardPageDown, Key.Pagedown},
        {UIKeyboardHidUsage.KeyboardPageUp, Key.Pageup},
        {UIKeyboardHidUsage.KeyboardDeleteForward, Key.Delete},
        {UIKeyboardHidUsage.KeyboardComma, Key.Comma},
        {UIKeyboardHidUsage.KeyboardPeriod, Key.Period},
        {UIKeyboardHidUsage.KeyboardSemicolon, Key.Semicolon},
        {UIKeyboardHidUsage.KeyboardOpenBracket, Key.LeftBracket},
        {UIKeyboardHidUsage.KeyboardCloseBracket, Key.RightBracket},
        {UIKeyboardHidUsage.KeyboardEqualSign, Key.Equals},
        {UIKeyboardHidUsage.KeyboardSeparator, Key.Minus}
    };
    
    private IApp _app;
    
    private readonly HashSet<UIKeyboardHidUsage> _pressedKeys = new();
    private int _nextTouchId = 1;
    private readonly Dictionary<IntPtr, int> _touchIds = new();
    
#if __MACCATALYST__
    public IPointingDevices PointingDevices => _pointingDevices ??= new PointingDevicesImpl(_metalView);
#else
    public IPointingDevices PointingDevices => _pointingDevices ??= new PointingDevicesImpl();
#endif
    
    private PointingDevicesImpl _pointingDevices = null;
    
    private readonly List<Action> _updateActions = new();
    private readonly List<Action> _tempUpdateActions = new();
    private readonly Stopwatch _stopwatch = new();
    
    private Vector2? _lastHoverPosition;
    private readonly UIHoverGestureRecognizer _hoverGestureRecognizer;
    public PixelViewController(MTKView metalView)
    {
        _metalView = metalView;
        _stopwatch.Start();

        _hoverGestureRecognizer = new UIHoverGestureRecognizer(() => { });
        _hoverGestureRecognizer.Enabled = true;
        _hoverGestureRecognizer.DelaysTouchesEnded = false;
        _hoverGestureRecognizer.DelaysTouchesBegan = false;
        _hoverGestureRecognizer.CancelsTouchesInView = false;
        
        View?.AddGestureRecognizer(_hoverGestureRecognizer);
    }

    private bool HoverEvent(UIGestureRecognizer _, UIEvent @event)
    {
        foreach (var t in @event.AllTouches)
        {
            var touch = (UITouch)t;

            var pt = touch.LocationInView(View);
            var position = new Vector2((float) pt.X, (float) pt.Y) * (float)UIScreen.MainScreen.NativeScale;
            
            switch (touch.Phase)
            {
                case UITouchPhase.RegionEntered:
                    _lastHoverPosition = position;
                    return true;
                
                case UITouchPhase.RegionMoved:
                    _lastHoverPosition = position;
                    return true;
                
                case UITouchPhase.Moved:
                    _lastHoverPosition = position;
                    return true;
                
                case UITouchPhase.RegionExited:
                    _lastHoverPosition = null;
                    return true;
            }
        }
        return true;
    }

    public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
    {
        ProcessPress(presses, evt);
    }

    public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
    {
        ProcessPress(presses, evt);
    }

    public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
    {
        ProcessPress(presses, evt);
    }

    public override void PressesChanged(NSSet<UIPress> presses, UIPressesEvent evt)
    {
        ProcessPress(presses, evt);
    }

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);
        ProcessTouch(touches, evt);
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
        base.TouchesMoved(touches, evt);
        ProcessTouch(touches, evt);
    }

    public override void TouchesCancelled(NSSet touches, UIEvent evt)
    {
        base.TouchesCancelled(touches, evt);
        ProcessTouch(touches, evt);
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
        base.TouchesEnded(touches, evt);
        ProcessTouch(touches, evt);
    }

    private void ProcessTouch(NSSet touches, UIEvent evt)
    {
        if (touches.Count == 0) return;
        
        foreach (var t in touches)
        {
            if (t is UITouch touch)
            {
                ProcessTouch(touch);
            }
        }
    }

    private int GetTouchId(IntPtr handle)
    {
        if (!_touchIds.TryGetValue(handle, out var touchId))
        {
            touchId = _nextTouchId++;
            _touchIds.Add(handle, touchId);
        }

        return touchId;
    }

    private void RemoveTouchId(IntPtr handle) => _touchIds.Remove(handle);

    private void ProcessTouch(UITouch touch)
    {
        var id = GetTouchId(touch.Handle.Handle);
        var pt = touch.LocationInView(View);
        var position = new Vector2((float)pt.X, (float)pt.Y) * (float)(View?.ContentScaleFactor ?? 1f);
        
        
        switch (touch.Phase)
        {
            case UITouchPhase.Began:
                _pointingDevices.TouchProcessor.AddEvent(TouchProcessor.TouchEventKind.Down, id, position, _stopwatch.Elapsed.TotalSeconds);
                break;

            case UITouchPhase.Moved:
                _pointingDevices.TouchProcessor.AddEvent(TouchProcessor.TouchEventKind.Move, id, position, _stopwatch.Elapsed.TotalSeconds);
                break;

            case UITouchPhase.Cancelled:
                _pointingDevices.TouchProcessor.AddEvent(TouchProcessor.TouchEventKind.Cancel, id, Vector2.Zero, _stopwatch.Elapsed.TotalSeconds);
                RemoveTouchId(touch.Handle.Handle);
                break;

            case UITouchPhase.Ended:
                _pointingDevices.TouchProcessor.AddEvent(TouchProcessor.TouchEventKind.Up, id, position, _stopwatch.Elapsed.TotalSeconds);
                RemoveTouchId(touch.Handle.Handle);
                break;
        }
    }

#if !__MACCATALYST__
    public override bool PrefersStatusBarHidden()
    {
        return true;
    }

    public override bool PrefersHomeIndicatorAutoHidden => true;
#endif

    private void ProcessPress(NSSet<UIPress> presses, UIPressesEvent evt)
    {
        foreach (var press in presses)
        {
            if (press.Key is null) continue;
            
            UIKeyboardHidUsage key = press.Key.KeyCode;

            if (press.Phase == UIPressPhase.Began)
            {
                _pressedKeys.Add(key);
            }
            else if (press.Phase != UIPressPhase.Changed)
            {
                _pressedKeys.Remove(key);
            }
        }
    }

    public void UpdateKeyboard(KeyboardImpl keyboard)
    {
        keyboard.UpdateButtons( set =>
        {
            foreach (var key in _pressedKeys)
            {
                if (KeyMapping.TryGetValue(key, out var keyVal))
                {
                    set.Add(keyVal);
                }
            }
        });
    }

    public void SetApp(IApp app) => _app = app;

    public void ProcessTouches()
    {
        if (_hoverGestureRecognizer.State is UIGestureRecognizerState.Began or UIGestureRecognizerState.Changed)
        {
            var pt = _hoverGestureRecognizer.LocationInView(View);
            var position = new Vector2((float)pt.X, (float)pt.Y) * (float)UIScreen.MainScreen.NativeScale;
            _pointingDevices.UpdateHoverPosition(position);
        }
        else
        {
            _pointingDevices.UpdateHoverPosition(null);
        }
        
        

        _pointingDevices.OnPreUpdate();
        _pointingDevices.TouchProcessor.ConsumeEvents();
    }
}

#endif