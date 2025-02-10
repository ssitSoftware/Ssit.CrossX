using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3.Graphics;

public unsafe class SdlSpriteRenderer(SDL_Renderer* renderer, IRenderStateProvider renderStateProvider) : ISpriteRenderer
{
    public void Draw(ITexture texture, RectangleF target, Rectangle? sourceRectangle = null, Vector2? nullableOrigin = null,
        RgbaColor? nullableColor = null, ImageTransform imageTransform = ImageTransform.None)
    {
        var scale = renderStateProvider.Scale;
        var offset = renderStateProvider.Offset;

        var (textureHandle, color) = renderStateProvider.GetProperTextureAndColor(texture, nullableColor);
        
        var source = sourceRectangle ?? new Rectangle(0, 0, texture.Size.Width, texture.Size.Height);
        
        var origin = nullableOrigin ?? new Vector2(texture.Size.Width / 2f, texture.Size.Height / 2f);
        
        SDL_FRect sourceRect = new()
        {
            x = source.X * scale + offset.X,
            y = source.Y * scale + offset.Y,
            w = source.Width * scale,
            h = source.Height * scale
        };

        SDL_FRect targetRect = new()
        {
            x = target.X * scale + offset.X,
            y = target.Y * scale + offset.Y,
            w = target.Width * scale,
            h = target.Height * scale
        };

        var scaleX = target.Width / source.Width;
        var scaleY = target.Height / source.Height;

        SDL_FPoint center = new()
        {
            x = targetRect.x + origin.X * scaleX * scale,
            y = targetRect.y + origin.Y * scaleY * scale
        };

        var flip = SDL_FlipMode.SDL_FLIP_NONE;
        double angle = 0;
        
        switch (imageTransform)
        {
            case ImageTransform.FlipHorizontal:
                flip = SDL_FlipMode.SDL_FLIP_HORIZONTAL;
                break;
            
            case ImageTransform.FlipVertical:
                flip = SDL_FlipMode.SDL_FLIP_VERTICAL;
                break;
            
            case ImageTransform.Rotate90:
                angle = Math.PI / 2;
                break;
            
            case ImageTransform.Rotate180:
                angle = Math.PI;
                break;
            
            case ImageTransform.Rotate270:
                angle = Math.PI * 3 / 2;
                break;
        }
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_RenderTextureRotated(renderer, textureHandle.Pointer,
            &sourceRect, &targetRect, angle, &center, flip);
    }

    public void Draw(ITexture texture, Vector2 position, Rectangle? sourceRectangle = null, Vector2? origin = null,
        float scale = 1, RgbaColor? color = null, ImageTransform imageTransform = ImageTransform.None)
    {
        var offset = origin ?? Vector2.Zero;
        position -= offset * scale;
        
        var targetRect = new RectangleF((int)position.X, (int)position.Y, texture.Size.Width * scale, texture.Size.Height * scale);
        Draw(texture, targetRect, sourceRectangle, origin, color, imageTransform);
    }

    public void Draw(SpriteInstance sprite, Vector2 position, float scale = 1, RgbaColor? color = null,
        ImageTransform transform = ImageTransform.None)
    {
        Draw(sprite.SpriteSheet, position, sprite.Source, sprite.Origin, scale, color, transform);
    }
}