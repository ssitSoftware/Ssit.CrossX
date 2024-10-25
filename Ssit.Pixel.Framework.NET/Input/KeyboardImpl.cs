using Ssit.Pixel.Framework.Input;
using static SDL2.Bindings.SDL;

namespace Ssit.Pixel.Framework.NET.Input;

internal class KeyboardImpl: IKeyboard
{
    private readonly bool[] _buttons = new bool[(int)Key.Max];
    private readonly bool[] _previousButtons = new bool[(int)Key.Max];
    
    public ButtonState GetKey(Key key)
    {
        bool isDown = _buttons[(int) key];
        bool wasDown = _previousButtons[(int) key];
        return new ButtonState(isDown, isDown != wasDown);
    }

    public void PreUpdate()
    {
        Array.Copy(_buttons, _previousButtons, _buttons.Length);
        
        var keysPtr = SDL_GetKeyboardState(out var count);
        count = Math.Min(count, _buttons.Length);

        unsafe
        {
            byte* ptr = (byte*) keysPtr;
            
            if (ptr == null) throw new InvalidProgramException();
            
            for (var idx = 0; idx < count; ++idx)
            {
                _buttons[idx] = ptr[idx] == 1;
            }
        }
    }
}