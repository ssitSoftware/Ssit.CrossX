using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace SampleGame.UI.Views;

public class PointsView: View
{
    public string Path { get; set; }
    public SharedValue<int> Points { get; set; }
    public SharedValue<int> MaxPoints { get; set; }
    public Length Spacing { get; set; }
}