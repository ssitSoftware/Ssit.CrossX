using System;
using System.Numerics;
using System.Text;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics.Internal;

public abstract class RendererBase : IRenderer
{
    protected enum BatchMode
    {
        None,
        ColorBuffer,
        TextureBuffer
    }
    
    protected DynamicVertexBuffer<VertexPositionColor> ColorVertexBuffer { get; } = new(1024, VertexPositionColor.Size);
    protected DynamicVertexBuffer<VertexPositionColorTexture> TextureVertexBuffer { get; } = new(1024, VertexPositionColorTexture.Size);
    
    protected BatchMode CurrentBatchMode { get; set; }
    
    public abstract Size TargetSize { get; }

    protected Matrix4x4? WorldTransform { get; set; }

    protected BlendMode _blendMode = BlendMode.AlphaBlend;
    
    public abstract void SetRenderTarget(IRenderTarget renderTarget);

    public void SetBlendMode(BlendMode blendMode)
    {
        _blendMode = blendMode;
    }

    public void SetTransform(Matrix3x2? matrix)
    {
        Flush();

        if (matrix.HasValue)
        {
            var m = matrix.Value;
            WorldTransform = new Matrix4x4(
                m.M11, m.M12, 0, 0, 
                m.M21, m.M22, 0, 0, 
                0, 0, 1, 0, 
                m.M31, m.M32, 0, 1);
        }
        else
        {
            WorldTransform = null;
        }
    }
    
    public void SetTransform(Matrix4x4 matrix)
    {
        Flush();
        WorldTransform = matrix;
    }

    public abstract void Clear(RgbaColor color);
    public abstract void Flush();

    protected abstract void PrepareRendering(ITexture texture, IEffect effect, VertexMode vertexMode,
        TextureFilter filter);
    
    public void DrawText(IFont font, string text, Vector2 position, RgbaColor? color = null, TextSpacing spacing = TextSpacing.Normal, float depth = 0, RgbaColor? outlineColor = null)
    {
        if (font is not IGlyphFont glyphFont)
        {
            return;
        }
        
        GlyphFontRenderer.RenderText(this, glyphFont, new TextSource
        {
            String = text
        }, position, color ?? RgbaColor.White, outlineColor ?? RgbaColor.Black, spacing, depth);
    }

    public void DrawText(IFont font, StringBuilder text, Vector2 position, RgbaColor? color = null, TextSpacing spacing = TextSpacing.Normal, float depth = 0, RgbaColor? outlineColor = null)
    {
        if (font is not IGlyphFont glyphFont)
        {
            return;
        }
        
        GlyphFontRenderer.RenderText(this, glyphFont, new TextSource
        {
            Builder = text
        }, position, color ?? RgbaColor.White, outlineColor ?? RgbaColor.Black, spacing, depth);
    }

    public void DrawText(IFont font, ICharProvider text, Vector2 position, RgbaColor? color = null, TextSpacing spacing = TextSpacing.Normal, float depth = 0, RgbaColor? outlineColor = null)
    {
        if (font is not IGlyphFont glyphFont)
        {
            return;
        }
        
        GlyphFontRenderer.RenderText(this, glyphFont, new TextSource
        {
            Provider = text
        }, position, color ?? RgbaColor.White, outlineColor ?? RgbaColor.Black, spacing, depth);
    }

    public virtual void DrawTexture(ITexture texture, Rectangle targetRectangle, 
        Rectangle? sourceRectangle = null, RgbaColor? color = null, 
        TextureFilter textureFilter = TextureFilter.Nearest, IEffect effect = null, float depth = 0)
    {
        if (CurrentBatchMode != BatchMode.TextureBuffer)
        {
            Flush();
        }

        if (TextureVertexBuffer.Offset + 6 > TextureVertexBuffer.Size)
        {
            Flush();
        }
        
        PrepareRendering(texture, effect, VertexPositionColorTexture.Mode, textureFilter);

        CurrentBatchMode = BatchMode.TextureBuffer;

        var col = color ?? RgbaColor.White;

        var t00 = new Vector2(0, 0);
        var t10 = new Vector2(1, 0);
        var t11 = new Vector2(1, 1);
        var t01 = new Vector2(0, 1);

        if (sourceRectangle.HasValue)
        {
            t00 = new Vector2((float) sourceRectangle.Value.X / texture.Size.Width, (float) sourceRectangle.Value.Y / texture.Size.Height);
            t10 = new Vector2((float) sourceRectangle.Value.Right / texture.Size.Width, (float) sourceRectangle.Value.Y / texture.Size.Height);
            t11 = new Vector2((float) sourceRectangle.Value.Right / texture.Size.Width, (float) sourceRectangle.Value.Bottom / texture.Size.Height);
            t01 = new Vector2((float) sourceRectangle.Value.X / texture.Size.Width, (float) sourceRectangle.Value.Bottom / texture.Size.Height);
        }
        
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y, depth), col, t00));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y + targetRectangle.Height, depth), col, t01));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y + targetRectangle.Height, depth), col, t11));
        
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y, depth), col, t00));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y + targetRectangle.Height, depth), col, t11));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y, depth), col, t10));
    }

    public virtual void DrawTexture(ITexture texture, Vector2 position, Rectangle? sourceRectangle = null, Vector2? origin = null,
        float rotation = 0, float scale = 1, RgbaColor? color = null,
        TextureFilter textureFilter = TextureFilter.Nearest,
        IEffect effect = null, float depth = 0)
    {
        if (CurrentBatchMode != BatchMode.TextureBuffer)
        {
            Flush();
        }

        if (TextureVertexBuffer.Offset + 6 > TextureVertexBuffer.Size)
        {
            Flush();
        }
        
        PrepareRendering(texture, effect, VertexPositionColorTexture.Mode, textureFilter);

        CurrentBatchMode = BatchMode.TextureBuffer;

        var col = color ?? RgbaColor.White;

        var t00 = new Vector2(0, 0);
        var t10 = new Vector2(1, 0);
        var t11 = new Vector2(1, 1);
        var t01 = new Vector2(0, 1);

        if (sourceRectangle.HasValue)
        {
            t00 = new Vector2((float) sourceRectangle.Value.X / texture.Size.Width, (float) sourceRectangle.Value.Y / texture.Size.Height);
            t10 = new Vector2((float) sourceRectangle.Value.Right / texture.Size.Width, (float) sourceRectangle.Value.Y / texture.Size.Height);
            t11 = new Vector2((float) sourceRectangle.Value.Right / texture.Size.Width, (float) sourceRectangle.Value.Bottom / texture.Size.Height);
            t01 = new Vector2((float) sourceRectangle.Value.X / texture.Size.Width, (float) sourceRectangle.Value.Bottom / texture.Size.Height);
        }

        position -= origin ?? Vector2.Zero;
        var targetRectangle = new RectangleF(position.X, position.Y, sourceRectangle?.Width ?? texture.Size.Width, sourceRectangle?.Height ?? texture.Size.Height);
        
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y, depth), col, t00));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y + targetRectangle.Height, depth), col, t01));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y + targetRectangle.Height, depth), col, t11));
        
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y, depth), col, t00));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y + targetRectangle.Height, depth), col, t11));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y, depth), col, t10));
    }

    public virtual void FillRectangle(RectangleF rectangle, RgbaColor color, float depth = 0)
    {
        if (CurrentBatchMode != BatchMode.ColorBuffer)
        {
            Flush();
        }

        if (ColorVertexBuffer.Offset + 6 > ColorVertexBuffer.Size)
        {
            Flush();
        }
        
        PrepareRendering(null, null, VertexPositionColor.Mode, TextureFilter.Nearest);
        
        CurrentBatchMode = BatchMode.ColorBuffer;
        
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y, depth), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y + rectangle.Height, depth), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, depth), color));
        
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y, depth), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, depth), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y, depth), color));
    }

    public abstract void DrawPrimitives(IVertexBuffer vertexBuffer, int vertexStart, int vertexCount,
        ITexture texture = null, RgbaColor? color = null, 
        TextureFilter textureFilter = TextureFilter.Nearest, IEffect effect = null);
}