using System;

namespace Ssit.CrossX.Input.Internal;

public abstract class KeyboardBase: IKeyboard
{
    public event Action<Key> OnKeyPressed;
    public event Action<Key> OnKeyReleased;

    protected void CallKeyPressed( Key key ) => OnKeyPressed?.Invoke(key);
    protected void CallKeyReleased( Key key ) => OnKeyReleased?.Invoke(key);
    
    public ButtonState GetKey(Key key)
    {
        if(key == Key.None)
            return ButtonState.Empty;
        
        return GetKeyInternal(key);
    }
    protected abstract ButtonState GetKeyInternal(Key key);
}