using System.Numerics;

namespace Nokemono.Core.Game.Objects;

public interface IHittable
{
    Vector2 Position { get; }
    bool Hit(Vector2 dir, float power);
}