using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IIndexedGeometryRenderer
{
    void DrawLine(Vector2 v1, Vector2 v2, byte color);
    void DrawRectangle(RectangleF rect, byte color);
    void FillRectangle(RectangleF rect, byte color);
    void DrawPoints(IReadOnlyList<Vector2> points, byte color);
}