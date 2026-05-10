using System.Collections.Generic;

namespace Ssit.CrossX.UI.Handlers.Markdown;

internal class LayoutLine
{
    public float Y;
    public float Height;
    public float Width;
    public bool IsLastInBlock;
    public List<LayoutPiece> Pieces = new();
}