using System;
using System.Collections.Generic;
using System.Numerics;
using SDL;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Graphics;

internal unsafe class SdlGeometryRenderer(SDL_Renderer* renderer, IRenderStateProvider renderStateProvider)
    : SdlRendererBase(renderStateProvider), IGeometryRenderer
{
    public int LinesRendered { get; private set; }
    public int RectanglesFilled { get; private set; }

    private SDL_Vertex[] _verticesArray = new SDL_Vertex[1]; 
    private SDL_FPoint[] _pointsArray = new SDL_FPoint[1];
    
    public void DrawLine(Vector2 v1, Vector2 v2, RgbaColor color)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        v1 = v1 * scale + offset;
        v2 = v2 * scale + offset;
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        
        SDL_RenderLine(renderer, v1.X, v1.Y, v2.X, v2.Y);
        
        LinesRendered++;
    }

    public void DrawPolyline(IReadOnlyList<Vector2> points, RgbaColor color)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;

        if (_pointsArray.Length < points.Count)
        {
            _pointsArray = new SDL_FPoint[points.Count];
        }
        
        for(var idx =0; idx < points.Count; idx++)
        {
            _pointsArray[idx] = new SDL_FPoint
            {
                x = points[idx].X * scale + offset.X,
                y = points[idx].Y * scale + offset.Y
            };
        }
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());

        fixed (SDL_FPoint* verticesPtr = _pointsArray)
        {
            SDL_RenderLines(renderer, verticesPtr, points.Count);
        }

        LinesRendered += points.Count-1;
    }
    
    public void DrawRectangle(RectangleF rect, RgbaColor color)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        SDL_FRect targetRect = new()
        {
            x = rect.X * scale + offset.X,
            y = rect.Y * scale + offset.Y,
            w = rect.Width * scale,
            h = rect.Height * scale
        };
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderRect(renderer, &targetRect);

        LinesRendered += 4;
    }
    
    public void DrawFrame(RectangleF rect, RgbaColor color, float thickness = 1)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
     
        int width = (int) MathF.Ceiling(thickness * scale);
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        
        SDL_FRect targetRect = new()
        {
            x = rect.X * scale + offset.X,
            y = rect.Y * scale + offset.Y,
            w = rect.Width * scale,
            h = rect.Height * scale
        };

        for (var idx = 0; idx < width; idx++)
        {
            SDL_RenderRect(renderer, &targetRect);
            LinesRendered += 4;
            
            targetRect.x += 1;
            targetRect.y += 1;
            targetRect.w -= 2;
            targetRect.h -= 2;
        }
    }

    public void FillRectangle(RectangleF rect, RgbaColor color)
    {
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        SDL_FRect targetRect = new()
        {
            x = rect.X * scale + offset.X,
            y = rect.Y * scale + offset.Y,
            w = rect.Width * scale,
            h = rect.Height * scale
        };
        
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        SDL_RenderFillRect(renderer, &targetRect);
        
        RectanglesFilled++;
    }

    public void DrawTriangles(IReadOnlyList<Vertex> vertices, int count = -1)
    {
        if (count == 0) return;
        count = count >= 0 ? count : vertices.Count;
        
        _verticesArray = Extensions.PrepareVertices(vertices, count, RenderStateProvider, _verticesArray);
        
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        
        fixed (SDL_Vertex* verticesPtr = _verticesArray)
        {
            SDL_RenderGeometry(renderer, null, verticesPtr, count, null, 0);
        }
    }
    
    public void DrawVertices(ITexture texture, IReadOnlyList<Vertex> vertices, int count = -1, RgbaColor? colorAttr = null)
    {
        if (count == 0) return;
        count = count >= 0 ? count : vertices.Count;
        
        var textureHandle = PrepareTextureRender(texture, colorAttr);
        _verticesArray = Extensions.PrepareVertices(vertices, count, RenderStateProvider, _verticesArray);
        
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        
        fixed (SDL_Vertex* verticesPtr = _verticesArray)
        {
            SDL_RenderGeometry(renderer, textureHandle.Pointer, verticesPtr, count, null, 0);
        }
    }

    public void DrawVertices(ITexture texture, IVertexBuffer vertexBuffer, int count = -1, RgbaColor? colorAttr = null)
    {
        if (count == 0) return;

        if (vertexBuffer is not SdlVertexBuffer sdlVertexBuffer)
        {
            return;
        }

        count = count >= 0 ? count : int.MaxValue;
        count = Math.Min(count, sdlVertexBuffer.VertexCount);
        
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        
        var textureHandle = PrepareTextureRender(texture, colorAttr);
        var vertices = sdlVertexBuffer.GetVertices(RenderStateProvider);
        
        fixed (SDL_Vertex* verticesPtr = vertices)
        {
            SDL_RenderGeometry(renderer, textureHandle.Pointer, verticesPtr, count, null, 0);
        }
    }

    public void DrawPoint(Vector2 position, RgbaColor color)
    {
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        var x = position.X * scale + offset.X;
        var y = position.Y * scale + offset.Y;
        
        SDL_RenderPoint(renderer, x, y);
    }
    
    public void DrawPoints(IReadOnlyList<Vector2> points, RgbaColor color)
    {
        SDL_SetRenderDrawColor(renderer, color.R, color.G, color.B, color.A);
        SDL_SetRenderDrawBlendMode(renderer, RenderStateProvider.BlendMode.ToSdlBlendMode());
        
        var scale = RenderStateProvider.Scale;
        var offset = RenderStateProvider.Offset;
        
        if (_pointsArray.Length < points.Count)
        {
            _pointsArray = new SDL_FPoint[points.Count];
        }
        
        for(var idx =0; idx < points.Count; idx++)
        {
            _pointsArray[idx] = new SDL_FPoint
            {
                x = points[idx].X * scale + offset.X,
                y = points[idx].Y * scale + offset.Y
            };
        }

        fixed (SDL_FPoint* verticesPtr = _pointsArray)
        {
            SDL_RenderPoints(renderer, verticesPtr, points.Count);    
        }
    }

    public void ResetStats()
    {
        RectanglesFilled = 0;
        LinesRendered = 0;
    }
}