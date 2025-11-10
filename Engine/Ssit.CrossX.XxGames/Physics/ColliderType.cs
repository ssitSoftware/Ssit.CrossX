using System;

namespace Ssit.CrossX.XxGames.Physics;

[Flags]
public enum ColliderType
{
    Static = 1,
    Dynamic = 2,
    Trigger = 4,
    Particle = 8
}