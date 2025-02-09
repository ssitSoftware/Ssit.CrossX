using System.Numerics;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IGeometryRenderer
{
    void DrawLine(Vector2 v1, Vector2 v2, RgbaColor color);
    void DrawRectangle(RectangleF rect, RgbaColor color);
    void FillRectangle(RectangleF rect, RgbaColor color);
}