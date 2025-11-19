using System.Numerics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface ITarget
{
    Vector2 Position { get; }
    ITarget Next { get; }
}