using System;

namespace Ssit.CrossX.Input;

/// <summary>
/// Provides methods to query the state of keys on a keyboard.
/// </summary>
public interface IKeyboard
{
    event Action<Key> OnKeyPressed;
    event Action<Key> OnKeyReleased;
    
    /// <summary>
    /// Retrieves the current state of the specified key.
    /// </summary>
    /// <param name="key">The key whose state is to be retrieved.</param>
    /// <returns>A <see cref="ButtonState"/> indicating the current state of the key.</returns>
    ButtonState GetKey(Key key);
}