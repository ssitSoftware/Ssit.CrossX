namespace Ssit.CrossX.UI.Parameters;

public struct Length(float value)
{
    public static readonly Length Auto = new(float.NaN);
    public static readonly Length Zero = new(0);
    
    public bool IsAuto => float.IsNaN(Value);
    
    private readonly float Value { get; } = value;

    public float Calculate(float reference = 0)
    {
        return IsAuto ? reference : Value;
    }
    
    public static implicit operator Length(float value) => new(value);
}