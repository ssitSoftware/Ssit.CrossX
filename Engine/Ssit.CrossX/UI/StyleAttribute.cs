using System;

namespace Ssit.CrossX.UI;

public class StyleAttribute(string name, Type targetType): Attribute
{
    public string Name { get; } = name;
    public Type TargetType { get; } = targetType;
}

public class StyleAttribute<TElement>(string name) : StyleAttribute(name, typeof(TElement));