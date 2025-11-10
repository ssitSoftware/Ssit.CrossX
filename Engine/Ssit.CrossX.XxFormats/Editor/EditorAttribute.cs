namespace Ssit.CrossX.XxFormats.Editor;

public class EditorAttribute : Attribute
{
    public Type HandlerType { get; }
    
    public EditorAttribute()
    {
    }

    public EditorAttribute(Type handlerType)
    {
        HandlerType = handlerType;
    }
}

public class EditorComplex : EditorAttribute
{
}