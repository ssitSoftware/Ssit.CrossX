using System;

namespace Ssit.CrossX.Games.Editor;

public class EditorLinkAttribute : EditorAttribute
{
    public Type Type { get; }

    public EditorLinkAttribute(Type types, Type validatorType = null): base(validatorType)
    {
        Type = types;
    }
}