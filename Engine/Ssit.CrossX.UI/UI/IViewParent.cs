using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI;

public interface IViewParent
{
    RectangleF ScreenBounds { get; }
    void RecalculateLayout(View view = null);
    RectangleF CalculateTargetBounds();
}