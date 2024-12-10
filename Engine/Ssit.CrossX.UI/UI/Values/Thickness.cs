using Ssit.CrossX.UI.Parameters;

namespace Ssit.CrossX.UI.Values;

public class Thickness
{
    public Length? Left { get; set; }
    public Length? Right { get; set; }
    public Length? Top { get; set; }
    public Length? Bottom { get; set; }
    
    public static implicit operator Thickness(Length? value) => new()
    {
        Left = value,
        Right = value,
        Top = value,
        Bottom = value
    };
    
    public static implicit operator Thickness((Length? horizontal, Length? vertical) th) => new()
    {
        Left = th.horizontal,
        Right = th.horizontal,
        Top = th.vertical,
        Bottom = th.vertical
    };
    
    public static implicit operator Thickness((Length? left, Length? top, Length? right, Length? bottom) th) => new()
    {
        Left = th.left,
        Right = th.right,
        Top = th.top,
        Bottom = th.bottom
    };
}