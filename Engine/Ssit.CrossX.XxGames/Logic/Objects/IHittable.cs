using System.Numerics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface IHittable
{
    Vector2 Position { get; }
    bool Hit(Vector2 dir, float power);
}