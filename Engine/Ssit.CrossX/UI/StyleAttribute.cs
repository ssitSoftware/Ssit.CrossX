using System;

namespace Ssit.CrossX.UI;

public class StyleAttribute(string name): Attribute
{
    public string Name { get; } = name;
}