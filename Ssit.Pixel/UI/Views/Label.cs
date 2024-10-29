using Ssit.Pixel.UI.Values;

namespace Ssit.Pixel.UI.Views;

public class Label: View
{
    public ISharedValue<string> Text { get; set; }
}