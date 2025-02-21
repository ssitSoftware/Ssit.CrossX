using System.Numerics;

namespace Ssit.CrossX.Games.Logic.Objects;

public interface ITarget
{
    Vector2 Position { get; }
    ITarget Next { get; }
}