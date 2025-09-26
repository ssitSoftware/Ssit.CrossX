using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Graphics.Internal;

public class IndexedGeometryRenderer(IRenderer2 renderer, IPaletteSource paletteSource) : IIndexedGeometryRenderer
{
    public void DrawLine(Vector2 v1, Vector2 v2, byte color) => renderer.GeometryRenderer.DrawLine(v1, v2, paletteSource.Palette[color]);
    public void DrawRectangle(RectangleF rect, byte color) => renderer.GeometryRenderer.DrawRectangle(rect, paletteSource.Palette[color]);
    public void FillRectangle(RectangleF rect, byte color) => renderer.GeometryRenderer.FillRectangle(rect, paletteSource.Palette[color]);
    public void DrawPoints(IReadOnlyList<Vector2> points, byte color) => renderer.GeometryRenderer.DrawPoints(points, paletteSource.Palette[color]);
}