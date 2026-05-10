using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class ImageView: Background
{
    public IImageSource Source { get; set; }
    public ContentAlign? ContentAlign { get; set; }
    public RgbaColor? TintColor { get; set; }
    public ImageScalingMode? Scaling { get; set; }
    public ImageTransform? Transform { get; set; }
    public TextureFilter? Filter { get; set; }
}
