namespace Ssit.Pixel.UI.Parameters;

public struct LengthSize
{
    public Length Width { get; }
    public Length Height { get; }

    public LengthSize(Length w, Length h)
    {
        Width = w;
        Height = h;
    }

    public static implicit operator LengthSize((Length w, Length h) val) => new(val.w, val.h);
}