using System;

namespace Ssit.CrossX.Input;

/// <summary>
/// Represents the state of a button, indicating whether it is pressed or not
/// and whether its state has changed.
/// </summary>
public readonly struct ButtonState : IEquatable<ButtonState>
{
    public bool Equals(ButtonState other)
    {
        return IsDown == other.IsDown && IsChanged == other.IsChanged;
    }

    public override bool Equals(object obj)
    {
        return obj is ButtonState other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IsDown, IsChanged);
    }

    /// <summary>
    /// Represents default button state (unpressed and not changed in current pass).
    /// </summary>
    public static readonly ButtonState Empty = new ();

    /// <summary>
    /// Represents a state where the button is already pressed.
    /// This state indicates that the button is pressed down
    /// and that there hasn't been a state change.
    /// </summary>
    public static readonly ButtonState Down = new (true, false);
    
    /// <summary>
    /// Represents a state where the button has just been pressed.
    /// This state indicates that the button is currently pressed down
    /// and that there has been a state change.
    /// </summary>
    public static readonly ButtonState JustPressed = new (true, true);

    /// <summary>
    /// Represents a button state where the button has just been released.
    /// The button is not pressed down and its state has changed.
    /// </summary>
    public static readonly ButtonState JustReleased = new (false, true);

    public static ButtonState FromStates(bool isDown, bool wasDown) => new ButtonState(isDown, isDown != wasDown);
    
    /// <summary>
    /// Represents the state of a button, indicating whether it is pressed or not
    /// and whether its state has changed.
    /// </summary>
    public ButtonState(bool isDown, bool isChanged)
    {
        IsDown = isDown;
        IsChanged = isChanged;
    }

    /// <summary>
    /// Indicates whether the button is currently pressed.
    /// </summary>
    public bool IsDown { get; }

    /// <summary>
    /// Indicates whether the button state has changed in the current pass.
    /// </summary>
    public bool IsChanged { get; }
    
    public static bool operator == (ButtonState b1, ButtonState b2)
    {
        return b1.IsChanged == b2.IsChanged && b1.IsDown == b2.IsDown;
    }

    public static bool operator !=(ButtonState b1, ButtonState b2)
    {
        return b1.IsChanged != b2.IsChanged || b1.IsDown != b2.IsDown;
    }
}