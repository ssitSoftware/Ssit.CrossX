using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.AabbPhysics;

public static class HelperExtansions
{
    public static Vector2 VelocitySum(this IBody body) => body.Velocity + body.KinematicVelocity;
}