using Ssit.CrossX.Graphics.Internal;

namespace Ssit.CrossX.Graphics.Renderer;

public class IndexedRenderer: IIndexedRenderer
{
    private readonly IRenderer2 _renderer;
    private readonly IPaletteSource _paletteSource;
    
    public IIndexedQuadsRenderer QuadsRenderer { get; }
    public IIndexedGeometryRenderer GeometryRenderer { get; }
    public IIndexedSpriteRenderer SpriteRenderer { get; }
    public IIndexedTextRenderer TextRenderer { get; }

    public IndexedRenderer(IRenderer2 renderer, IPaletteSource paletteSource)
    {
        _renderer = renderer;
        _paletteSource = paletteSource;

        QuadsRenderer = new IndexedQuadsRenderer(renderer);
        GeometryRenderer = new IndexedGeometryRenderer(renderer, paletteSource);
        SpriteRenderer = new IndexedSpriteRenderer(renderer);
        TextRenderer = new IndexedTextRenderer(renderer, paletteSource);
    }
    
    public void Clear(byte color)
    {
        var clr = _paletteSource.Palette[color];
        _renderer.Clear(clr);
    }
}