using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IGeometryRenderer
{
    int LinesRendered { get; }
    int RectanglesFilled { get; }
    
    void DrawLine(Vector2 v1, Vector2 v2, RgbaColor color);
    void DrawRectangle(RectangleF rect, RgbaColor color);
    void FillRectangle(RectangleF rect, RgbaColor color);
    void DrawVertices(ITexture texture, IReadOnlyList<Vertex> vertices, int count = -1, RgbaColor? color = null);
}