using System.Numerics;

namespace SampleGame.Game.Logic;

public interface ICamera
{
    Vector2 LookAt { get; }
}

public interface IHittable
{
    void Hit(float damage, Vector2 direction);
}