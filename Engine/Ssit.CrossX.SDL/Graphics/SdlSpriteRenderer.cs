using System.Numerics;
using SDL;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Graphics;

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
        
        Vector2 sc = new Vector2(targetRect.w / sourceRect.w, targetRect.h / sourceRect.h);
        
        var center = new SDL_FPoint
        {
            x = rotCenter.X * sc.X,
            y = rotCenter.Y * sc.Y
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
                angle = 90;
                break;
            
            case ImageTransform.Rotate180:
                angle = 180;
                break;
            
            case ImageTransform.Rotate270:
                angle = 270;
                break;
        }
        
        SDL_RenderTextureRotated(renderer, textureHandle.Pointer,
            &sourceRect, &targetRect, angle, &center, flip);
        
        SpritesRendered++;
    }

    public void Draw(ITexture texture, Vector2 position, RectangleF? sourceRectangle = null, Vector2? origin = null,
        float scale = 1, RgbaColor? color = null, ImageTransform imageTransform = ImageTransform.None)
    {
        Vector2 offset = Vector2.Zero;

        if (origin != null)
        {
            offset = origin.Value;

            if (imageTransform == ImageTransform.FlipHorizontal)
            {
                offset.X = (sourceRectangle?.Width ?? texture.Size.Width) - origin.Value.X;
            }
        }
        else if (sourceRectangle != null)
        {
            offset = (sourceRectangle.Value.Size / 2f).ToVector();
        }
        
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