namespace Ssit.Pixel.Input.Internal;

public abstract class KeyboardBase: IKeyboard
{
    public ButtonState GetKey(Key key) => GetKeyInternal(key);
    protected abstract ButtonState GetKeyInternal(Key key);
}