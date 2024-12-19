using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class ScrollView: Background
{
    public View ContentView { get; set; }
    public ScrollMode ScrollMode { get; set; }
}