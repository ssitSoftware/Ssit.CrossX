
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
    
    public SdlKeyboard()
    {
        int keyCount;
        _keys = SDL_GetKeyboardState(&keyCount);
        _keyCount = keyCount;
        
        _currentKeys = new bool[_keyCount];
        _previousKeys = new bool[_keyCount];
    }
    
    protected override ButtonState GetKeyInternal(Key key)
    {
        var index = (int)key;
        return new ButtonState(_currentKeys[index], _currentKeys[index] && !_previousKeys[index]);
    }

    public void Update()
    {
        (_currentKeys, _previousKeys) = (_previousKeys, _currentKeys);
        for (var idx = 0; idx < _keyCount; ++idx)
        {
            _currentKeys[idx] = _keys[idx] == true;
        }
    }
}