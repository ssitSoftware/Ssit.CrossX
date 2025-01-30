using System;
using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Graphics;

public class RendererStateManager
{
    private readonly struct RendererState(Matrix4x4? transform, RectangleF? clipRect)
    {
        public Matrix4x4? Transform { get; } = transform;
        public RectangleF? ClipRect { get; } = clipRect;
    }
    
    private readonly Stack<RendererState> _states = new();
    
    internal event Action ShouldFlush;
    internal Matrix4x4? WorldTransform { get; private set; }

    internal Rectangle? ClipRect
    {
        get
        {
            if (!_clipRect.HasValue)
                return null;

            var x = (int)MathF.Ceiling(_clipRect.Value.X);
            var y = (int)MathF.Ceiling(_clipRect.Value.Y);
            var rx = (int)MathF.Floor(_clipRect.Value.Right);
            var by = (int)MathF.Floor(_clipRect.Value.Bottom);
            
            return new Rectangle(x, y, rx - x, by - y);
        }
    }

    private RectangleF? _clipRect;
    
    public void RestoreState()
    {
        ShouldFlush?.Invoke();
        var state = _states.Pop();
        WorldTransform = state.Transform;
        _clipRect = state.ClipRect;
    }

    public void SaveState()
    {
        _states.Push(new RendererState(WorldTransform, _clipRect));
    }

    public void Transform(Matrix3x2 m)
    {
        Transform(new Matrix4x4(
            m.M11, m.M12, 0, 0,
            m.M21, m.M22, 0, 0,
            0, 0, 1, 0,
            m.M31, m.M32, 0, 1));
    }
    
    public void Transform(Matrix4x4 matrix)
    {
        if (WorldTransform.HasValue)
        {
            WorldTransform = WorldTransform.Value * matrix;
        }
        else
        {
            WorldTransform = matrix;
        }
    }

    public void ClipRectangle(RectangleF rect)
    {
        rect = TransformRect(rect);
        
        if (_clipRect.HasValue)
        {
            _clipRect = rect.Intersect(_clipRect.Value);
        }
        else
        {
            _clipRect = rect;
        }
    }

    private RectangleF TransformRect(RectangleF rect)
    {
        if (WorldTransform == null)
        {
            return rect;
        }
        
        Vector2 topLeft = Vector2.Transform(rect.TopLeft, WorldTransform.Value);
        Vector2 bottomRight = Vector2.Transform(rect.BottomRight, WorldTransform.Value);
        
        Vector2 topRight = Vector2.Transform(rect.TopRight, WorldTransform.Value);
        Vector2 bottomLeft = Vector2.Transform(rect.BottomLeft, WorldTransform.Value);

        var minX = MathF.Min(MathF.Min(topLeft.X, bottomRight.X), MathF.Min(topRight.X, bottomLeft.X));
        var maxX = MathF.Max(MathF.Max(topLeft.X, bottomRight.X), MathF.Max(topRight.X, bottomLeft.X));
        
        var minY = MathF.Min(MathF.Min(topLeft.Y, bottomRight.Y), MathF.Min(topRight.Y, bottomLeft.Y));
        var maxY = MathF.Max(MathF.Max(topLeft.Y, bottomRight.Y), MathF.Max(topRight.Y, bottomLeft.Y));
        
        return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    public void Reset()
    {
        ShouldFlush?.Invoke();
        _states.Clear();
        WorldTransform = null;
        _clipRect = null;
    }
}