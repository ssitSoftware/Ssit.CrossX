
using SDL;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Input;

public unsafe class SdlKeyboard: KeyboardBase
{
    private readonly int _keyCount;
    private readonly SDLBool* _keys;

    private bool[] _currentKeys;
    private bool[] _previousKeys;

    private bool[] _previousMouse = new bool[3];
    private bool[] _currentMouse = new bool[3];
    
    public SdlKeyboard()
    {
        int keyCount;
        _keys = SDL_GetKeyboardState(&keyCount);
        _keyCount = keyCount;
        
        _currentKeys = new bool[_keyCount];
        _previousKeys = new bool[_keyCount];
    }
    
    public void KeyPressed( Key key ) => CallKeyPressed(key);
    public void KeyReleased( Key key ) => CallKeyReleased(key);
    
    protected override ButtonState GetKeyInternal(Key key)
    {
        if (key >= Key.MouseButtonLeft)
        {
            var idx = (int)key - (int)Key.MouseButtonLeft;
            return new ButtonState(_currentMouse[idx], _currentMouse[idx] != _previousMouse[idx]);
        }
        
        var index = (int)key;
        return new ButtonState(_currentKeys[index], _currentKeys[index] != _previousKeys[index]);
    }

    public void Update()
    {
        (_currentKeys, _previousKeys) = (_previousKeys, _currentKeys);
        for (var idx = 0; idx < _keyCount; ++idx)
        {
            _currentKeys[idx] = _keys[idx] == true;
        }

        float x, y;
        var state = SDL_GetMouseState(&x, &y);

        _previousMouse[0] = _currentMouse[0];
        _previousMouse[1] = _currentMouse[1];
        _previousMouse[2] = _currentMouse[2];
        
        _currentMouse[0] = (state & SDL_MouseButtonFlags.SDL_BUTTON_LMASK) != 0;
        _currentMouse[1] = (state & SDL_MouseButtonFlags.SDL_BUTTON_RMASK) != 0;
        _currentMouse[2] = (state & SDL_MouseButtonFlags.SDL_BUTTON_MMASK) != 0;
    }
}