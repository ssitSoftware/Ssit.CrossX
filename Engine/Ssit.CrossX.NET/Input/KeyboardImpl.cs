using System;
using System.Collections.Generic;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.NET.Input;

internal class KeyboardImpl: IKeyboard
{
    private HashSet<Key> _buttons = new();
    private HashSet<Key> _previousButtons = new();
    
    public ButtonState GetKey(Key key)
    {
        bool isDown = _buttons.Contains(key);
        bool wasDown = _previousButtons.Contains(key);
        return new ButtonState(isDown, isDown != wasDown);
    }

    public void UpdateButtons(Action<HashSet<Key>> callback)
    {
        (_previousButtons, _buttons) = (_buttons, _previousButtons);
        
        _buttons.Clear();
        callback(_buttons);
    }
}