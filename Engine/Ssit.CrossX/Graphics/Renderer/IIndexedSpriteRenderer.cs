using System.Numerics;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IIndexedSpriteRenderer
{
    void Draw(ITexture texture, Vector2 position, RectangleF? sourceRectangle = null, Vector2? origin = null, 
        ImageTransform imageTransform = ImageTransform.None);
    void Draw(SpriteInstance sprite, Vector2 position, ImageTransform transform = ImageTransform.None);
}