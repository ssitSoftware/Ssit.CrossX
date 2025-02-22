using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3.Graphics;

public unsafe class SdlSpriteRenderer(SDL_Renderer* renderer, IRenderStateProvider renderStateProvider) 
    : SdlRendererBase(renderStateProvider) , ISpriteRenderer
{
    public int SpritesRendered { get; private set; }

    public void Draw(ITexture texture, RectangleF target, RectangleF? sourceRectangle = null, Vector2? nullableOrigin = null,
        RgbaColor? nullableColor = null, ImageTransform imageTransform = ImageTransform.None)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;

        var textureHandle = PrepareTextureRender(texture, nullableColor);
        
        var source = sourceRectangle ?? new RectangleF(0, 0, texture.Size.Width, texture.Size.Height);
        var rotCenter = nullableOrigin ?? new Vector2(texture.Size.Width / 2f, texture.Size.Height / 2f);
        
        SDL_FRect sourceRect = new()
        {
            x = source.X,
            y = source.Y,
            w = source.Width,
            h = source.Height
        };

        SDL_FRect targetRect = new()
        {
            x = target.X * scale + offset.X,
            y = target.Y * scale + offset.Y,
            w = target.Width * scale,
            h = target.Height * scale
        };
        
        var center = new SDL_FPoint
        {
            x = sourceRect.x + rotCenter.X,
            y = sourceRect.y + rotCenter.Y
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
        
        SDL_RenderTextureRotated(renderer, textureHandle.Pointer,
            &sourceRect, &targetRect, angle, &center, flip);

        SpritesRendered++;
    }

    public void Draw(ITexture texture, Vector2 position, RectangleF? sourceRectangle = null, Vector2? origin = null,
        float scale = 1, RgbaColor? color = null, ImageTransform imageTransform = ImageTransform.None)
    {
        var offset = origin ?? Vector2.Zero;
        position -= offset * scale;
        
        var sourceRect = sourceRectangle ?? new RectangleF(0, 0, texture.Size.Width, texture.Size.Height);
        
        var targetRect = new RectangleF(position.X, position.Y, sourceRect.Width * scale, sourceRect.Height * scale);
        Draw(texture, targetRect, sourceRectangle, origin, color, imageTransform);
    }

    public void Draw(SpriteInstance sprite, Vector2 position, float scale = 1, RgbaColor? color = null,
        ImageTransform transform = ImageTransform.None)
    {
        Draw(sprite.SpriteSheet, position, sprite.Source, sprite.Origin, scale, color, transform);
    }
    
    public void ResetStats() => SpritesRendered = 0;
}