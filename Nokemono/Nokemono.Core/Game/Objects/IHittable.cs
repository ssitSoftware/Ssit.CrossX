using System.Numerics;

namespace Nokemono.Core.Game.Objects;

public interface IHittable
{
    void Hit(Vector2 dir, float power);
}