namespace Ssit.CrossX.UI.Parameters;

public struct Length
{
    public float Value { get; }

    public Length(float value)
    {
        Value = value;
    }
    
    public static implicit operator Length(float value) => new(value);
}