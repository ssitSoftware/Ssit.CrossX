using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Graphics.Internal;
using Ssit.CrossX.IoC;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics;

public static class RendererExtensions
{
    public static TextRenderingContext CalculateMultilineText(this IFont font, TextSource text,
        TextAlign align, TextSpacing spacing, float maxWidth, float paragraphSpacing, TextRenderingContext context = null)
    {
        if(font is not IGlyphFont glyphFont) throw new NotSupportedException("This kind of font is not supported");

        context ??= new TextRenderingContext();
        
        GlyphFontRenderer.CalculateMultilineText(glyphFont, text, maxWidth, align, spacing, paragraphSpacing, context);
        return context;
    }
    
    public static TextRenderingContext CalculateText(this IFont font, TextSource text, TextSpacing spacing, TextRenderingContext context = null)
    {
        if(font is not IGlyphFont glyphFont) throw new NotSupportedException("This kind of font is not supported");

        context ??= new TextRenderingContext();
        
        GlyphFontRenderer.CalculateText(glyphFont, text, spacing, context);
        return context;
    }

    public static IVertexBuffer[] CreateMultilineTextPrimitives(this IGlyphFont font, IIoCContainer container , TextSource text, RectangleF target,
        TextAlign align, TextSpacing spacing, float paragraphSpacing, float depth = 0)
    {
        const int maxBufferSize = 60000;
        
        var context = CalculateMultilineText(font, text, align, spacing, target.Width, paragraphSpacing);
        
        var list = new List<VertexPositionColorTexture>();
        
        var position = target.TopLeft;
        
        if ((align & TextAlign.Center) == TextAlign.Center)
        {
            position.X = target.Center.X;
        }
        
        if ((align & TextAlign.VCenter) == TextAlign.VCenter)
        {
            position.Y = target.Center.Y;
        }
        
        if ((align & TextAlign.Right) == TextAlign.Right)
        {
            position.X = target.Right;
        }
        
        if ((align & TextAlign.Bottom) == TextAlign.Bottom)
        {
            position.Y = target.Bottom;
        }
        
        void DrawAction(ITexture texture, Rectangle targetRect, Rectangle source, RgbaColor color, float depth0)
        {
            var t00 = new Vector2((float)source.X / texture.Size.Width, (float)source.Y / texture.Size.Height);
            var t10 = new Vector2((float)source.Right / texture.Size.Width, (float)source.Y / texture.Size.Height);
            var t11 = new Vector2((float)source.Right / texture.Size.Width, (float)source.Bottom / texture.Size.Height);
            var t01 = new Vector2((float)source.X / texture.Size.Width, (float)source.Bottom / texture.Size.Height);

            list.Add(new VertexPositionColorTexture(new Vector3(targetRect.X, targetRect.Y, depth0), color, t00));
            list.Add(new VertexPositionColorTexture(new Vector3(targetRect.X, targetRect.Y + targetRect.Height, depth0), color, t01));
            list.Add(new VertexPositionColorTexture(new Vector3(targetRect.X + targetRect.Width, targetRect.Y + targetRect.Height, depth0), color, t11));

            list.Add(new VertexPositionColorTexture(new Vector3(targetRect.X, targetRect.Y, depth0), color, t00));
            list.Add(new VertexPositionColorTexture(new Vector3(targetRect.X + targetRect.Width, targetRect.Y + targetRect.Height, depth0), color, t11));
            list.Add(new VertexPositionColorTexture(new Vector3(targetRect.X + targetRect.Width, targetRect.Y, depth0), color, t10));
        }
        
        GlyphFontRenderer.RenderText(DrawAction, font.FontSheet, context.Font, context.Lines, position, align, RgbaColor.White, spacing, context.Width, context.Height, depth);

        var buffers = (int)Math.Ceiling(list.Count / (double)maxBufferSize);
        var vertexBuffers = new IVertexBuffer[buffers];
        
        for (var idx = 0; idx < buffers; ++idx)
        {
            vertexBuffers[idx] = container.IoCConstruct<IVertexBuffer>(new CreatePctVertexBufferParameters
            {
                Vertices = list.Skip(idx * maxBufferSize).Take(maxBufferSize).ToArray()
            });
        }

        return vertexBuffers;
    }

    public static void RenderVertexBuffers(this IRenderer renderer, IReadOnlyList<IVertexBuffer> buffers, 
        ITexture texture = null, RgbaColor? color = null, TextureFilter filter = TextureFilter.Nearest, 
        Matrix4x4? transform = null, 
        IEffect effect = null)
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var idx = 0; idx < buffers.Count; ++idx)
        {
            renderer.DrawPrimitives(buffers[idx], 0, buffers[idx].Length, texture, color, filter, transform, effect);    
        }
    }
}