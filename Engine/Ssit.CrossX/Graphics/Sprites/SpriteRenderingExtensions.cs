using System.Numerics;

namespace Ssit.CrossX.Graphics.Sprites;

public static class SpriteRenderingExtensions
{
    public static void DrawSprite(this IRenderer renderer, SpriteInstance sprite, Vector2 position, float scale = 1, RgbaColor? color = null, ImageTransform transform = ImageTransform.None)
    {
        var origin = sprite.Origin;

        switch (transform)
        {
            case ImageTransform.FlipHorizontal:
                origin.X = sprite.Source.Width - origin.X;
                break;
            
            case ImageTransform.FlipVertical:
                origin.Y = sprite.Source.Width - origin.Y;
                break;
        }
        
        renderer.DrawTexture(sprite.SpriteSheet, position, sprite.Source, origin, 0, scale, color, transform);
    }
}