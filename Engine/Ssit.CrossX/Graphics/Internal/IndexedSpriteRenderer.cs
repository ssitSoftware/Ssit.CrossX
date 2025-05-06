using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Graphics.Internal;

public class IndexedSpriteRenderer(IRenderer2 renderer) : IIndexedSpriteRenderer
{
    public void Draw(ITexture texture, Vector2 position, RectangleF? sourceRectangle = null, Vector2? origin = null,
        ImageTransform imageTransform = ImageTransform.None) =>
        renderer.SpriteRenderer.Draw(texture, position, sourceRectangle, origin, imageTransform: imageTransform);

    public void Draw(SpriteInstance sprite, Vector2 position, ImageTransform transform = ImageTransform.None) 
        => renderer.SpriteRenderer.Draw(sprite, position, 1, null, transform);
}