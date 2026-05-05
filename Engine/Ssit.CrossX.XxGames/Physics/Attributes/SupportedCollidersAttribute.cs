using System;

namespace Ssit.CrossX.XxGames.Physics.Attributes;

public class SupportedCollidersAttribute(params Type[] types) : Attribute
{
    public Type[] Types { get; } = types;
}