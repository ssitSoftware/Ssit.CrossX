namespace Ssit.CrossX.UI;

public interface IFocusable
{
    bool IsFocused { get; }
    void Focus();
    RectangleF Bounds { get; }
}