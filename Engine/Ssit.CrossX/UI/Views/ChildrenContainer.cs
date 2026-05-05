using System.Collections.Generic;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public abstract class ChildrenContainer : Background
{
    public IList<View> Children { get; set; } = [];
    public SignalSource<View> LayoutSignal { get; set; }
    public Thickness? Padding { get; set; }
}