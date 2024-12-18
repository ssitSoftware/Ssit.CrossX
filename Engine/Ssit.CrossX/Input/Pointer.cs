using System.Numerics;

namespace Ssit.CrossX.Input;

public sealed class Pointer(int id)
{
    public int Id { get; } = id;
    
    public ButtonState State { get; private set; } = ButtonState.Empty;
    public Vector2 Position { get; private set; } = Vector2.Zero;
    public Vector2 Origin { get; private set; } = Vector2.Zero;

    internal void Update(ButtonState state, Vector2 position, Vector2 origin)
    {
        State = state;
        Position = position;
        Origin = origin;
    }
    
    internal void Update(ButtonState state, Vector2 position)
    {
        State = state;
        Position = position;
    }
    
    internal void Update(ButtonState state)
    {
        State = state;
    }

    internal void Reset()
    {
        State = ButtonState.Empty;
        Position = Vector2.Zero;
        Origin = Vector2.Zero;
    }
}