using System.Numerics;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Graphics.Renderer;

public interface ISpriteRenderer
{
    int SpritesRendered { get; }
    
    void Draw(ITexture texture, RectangleF target, RectangleF? sourceRectangle = null, Vector2? origin = null,
        RgbaColor? nullableColor = null, ImageTransform imageTransform = ImageTransform.None);
    
    void Draw(ITexture texture, Vector2 position, RectangleF? sourceRectangle = null,
        Vector2? origin = null, float scale = 1, RgbaColor? color = null,
        ImageTransform imageTransform = ImageTransform.None);

    void Draw(SpriteInstance sprite, Vector2 position, float scale = 1, RgbaColor? color = null,
        ImageTransform transform = ImageTransform.None);
}