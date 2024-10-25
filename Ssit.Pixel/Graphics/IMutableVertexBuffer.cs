using System.Collections.Generic;
using System.Numerics;

namespace Ssit.Pixel.Graphics;

public interface IMutableVertexBuffer: IVertexBuffer
{
    void UpdateVertices(IReadOnlyList<Vector2> positions, IReadOnlyList<Vector2> coordinates, IReadOnlyList<RgbaColor> colors = null);
}