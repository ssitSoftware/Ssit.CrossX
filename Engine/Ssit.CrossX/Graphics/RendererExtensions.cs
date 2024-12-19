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
    public static TextRenderingContext CalculateMultilineText(this IFont font, TextSource text, TextSpacing spacing, float maxWidth, float paragraphSpacing, TextRenderingContext context = null)
    {
        if(font is not IGlyphFont glyphFont) throw new NotSupportedException("This kind of font is not supported");

        context ??= new TextRenderingContext();
        
        GlyphFontRenderer.CalculateMultilineText(glyphFont, text, maxWidth, spacing, paragraphSpacing, context);
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
        ContentAlign align, TextSpacing spacing, float paragraphSpacing, float depth = 0, TextRenderingContext context = null, IVertexBuffer[] oldBuffers = null)
    {
        const int maxBufferSize = 60000;
        
        context = CalculateMultilineText(font, text, spacing, target.Width, paragraphSpacing, context);
        
        var list = new List<VertexPositionColorTexture>();
        
        var position = target.TopLeft;
        
        if ((align & ContentAlign.Center) == ContentAlign.Center)
        {
            position.X = target.Center.X;
        }
        
        if ((align & ContentAlign.VCenter) == ContentAlign.VCenter)
        {
            position.Y = target.Center.Y;
        }
        
        if ((align & ContentAlign.Right) == ContentAlign.Right)
        {
            position.X = target.Right;
        }
        
        if ((align & ContentAlign.Bottom) == ContentAlign.Bottom)
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
        
        GlyphFontRenderer.RenderText(DrawAction, font.FontSheet, context.Font, context.Lines, position, align, RgbaColor.White, spacing, context.TargetWidth, context.Height, depth);

        var buffers = (int)Math.Ceiling(list.Count / (double)maxBufferSize);

        IVertexBuffer[] vertexBuffers = null;
        
        if (oldBuffers != null)
        {
            if (oldBuffers.Length == buffers)
            {
                var total = list.Count;
                bool isValid = true;
                for (var idx = 0; idx < buffers; idx++)
                {
                    var size = total;
                    size = Math.Min(size, maxBufferSize);
                    total -= maxBufferSize;
                    
                    if (oldBuffers[idx].Length != size)
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    vertexBuffers = oldBuffers;
                }
                else
                {
                    foreach (var ob in oldBuffers)
                    {
                        ob.Dispose();
                    }
                }
            }
        }
        
        if (vertexBuffers == null)
        {
            vertexBuffers = new IVertexBuffer[buffers];   
        }
        
        for (var idx = 0; idx < buffers; ++idx)
        {
            if (vertexBuffers[idx] != null)
            {
                vertexBuffers[idx].Update(list.Skip(idx * maxBufferSize).Take(maxBufferSize).ToArray());
            }
            else
            {
                vertexBuffers[idx] = container.IoCConstruct<IVertexBuffer>(new CreatePctVertexBufferParameters
                {
                    Vertices = list.Skip(idx * maxBufferSize).Take(maxBufferSize).ToArray()
                });
            }
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