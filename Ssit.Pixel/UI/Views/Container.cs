using System.Collections.Generic;

namespace Ssit.Pixel.UI.Views;

public class Container: View
{
    public IList<View> Children { get; set; }
}