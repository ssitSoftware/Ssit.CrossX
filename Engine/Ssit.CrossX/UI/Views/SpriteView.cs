using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class SpriteView : View
{
    public string SpritePath { get; set; }
    public SharedString Sequence { get; set; }
    public float SpeedFactor { get; set; } = 1.0f;
    public Length? ImageAnchorX { get; set; }
    public Length? ImageAnchorY { get; set; }
    public int Scale { get; set; } = 1;
}
