using System.Numerics;

namespace Gunslinger.Core.Game.Objects;

public interface ITarget
{
    Vector2 Position { get; }
    ITarget Next { get; }
}