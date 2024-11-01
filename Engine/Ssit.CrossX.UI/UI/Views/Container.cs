using System.Collections.Generic;

namespace Ssit.CrossX.UI.Views;

public class Container: View
{
    public IList<View> Children { get; set; }
}