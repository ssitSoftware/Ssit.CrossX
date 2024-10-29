namespace Ssit.Pixel.UI.Parameters;

public struct LengthPosition
{
    public Length X { get; }
    public Length Y { get; }

    public LengthPosition(Length x, Length y)
    {
        X = x;
        Y = y;
    }
    
    public static implicit operator LengthPosition((Length x, Length y) val) => new(val.x, val.y);
}