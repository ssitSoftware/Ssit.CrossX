#if __IOS__ || __MACCATALYST__

using System.Collections.Generic;
using Foundation;
using Ssit.Pixel.Core;
using Ssit.Pixel.Input;
using Ssit.Pixel.NET.Input;
using UIKit;

namespace Ssit.Pixel.NET.Apple;

internal class PixelViewController : UIViewController
{
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
    
    private HashSet<UIKeyboardHidUsage> _pressedKeys = new();
    
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
}

#endif