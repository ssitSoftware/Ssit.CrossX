namespace Ssit.CrossX.Graphics.Renderer;

public interface IIndexedRenderer
{
    IIndexedQuadsRenderer QuadsRenderer { get; }
    IIndexedGeometryRenderer GeometryRenderer { get; }
    IIndexedSpriteRenderer SpriteRenderer { get; }
    IIndexedTextRenderer TextRenderer { get; }
    void Clear(byte color);
}