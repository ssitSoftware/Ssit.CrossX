namespace Ssit.CrossX.XxFormats.Editor;

public class EditorLinkAttribute : EditorAttribute
{
    public Type Type { get; }

    public EditorLinkAttribute(Type type, Type validatorType = null): base(validatorType)
    {
        Type = type;
    }
}