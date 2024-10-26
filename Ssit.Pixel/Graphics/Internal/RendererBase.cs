using System;
using System.Numerics;
using System.Text;

namespace Ssit.Pixel.Graphics.Internal;

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

    public abstract void Clear(RgbaColor color);
    public abstract void Flush();

    protected abstract void PrepareRendering(ITexture texture, IEffect effect, VertexMode vertexMode, TextureFilter filter);
    
    public virtual void DrawText(IFont font, string text, Vector2 position, RgbaColor? color = null)
    {
        throw new NotImplementedException();
    }

    public virtual void DrawText(IFont font, StringBuilder text, Vector2 position, RgbaColor? color = null)
    {
        throw new NotImplementedException();
    }

    public virtual void DrawTexture(ITexture texture, Rectangle targetRectangle, 
        Rectangle? sourceRectangle = null, RgbaColor? color = null, 
        TextureFilter textureFilter = TextureFilter.Nearest, IEffect effect = null)
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
        
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y, 0f), col, t00));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y + targetRectangle.Height, 0f), col, t01));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y + targetRectangle.Height, 0f), col, t11));
        
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X, targetRectangle.Y, 0f), col, t00));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y + targetRectangle.Height, 0f), col, t11));
        TextureVertexBuffer.AddVertex(new VertexPositionColorTexture(new Vector3(targetRectangle.X + targetRectangle.Width, targetRectangle.Y, 0f), col, t10));
    }

    public virtual void DrawTexture(ITexture texture, Vector2 position, Rectangle? sourceRectangle = null, Vector2? origin = null,
        float rotation = 0, float scale = 1, RgbaColor? color = null, RenderTransform transform = RenderTransform.None,
        TextureFilter textureFilter = TextureFilter.Nearest,
        IEffect effect = null)
    {
    }

    public virtual void FillRectangle(RectangleF rectangle, RgbaColor color)
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
        
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y, 0f), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y + rectangle.Height, 0f), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0f), color));
        
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X, rectangle.Y, 0f), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, 0f), color));
        ColorVertexBuffer.AddVertex(new VertexPositionColor(new Vector3(rectangle.X + rectangle.Width, rectangle.Y, 0f), color));
    }

    public abstract void DrawPrimitives(IVertexBuffer vertexBuffer, int vertexStart, int vertexCount,
        ITexture texture = null,
        RgbaColor? color = null, Matrix3x2? transform = null, 
        TextureFilter textureFilter = TextureFilter.Nearest, IEffect effect = null);
}