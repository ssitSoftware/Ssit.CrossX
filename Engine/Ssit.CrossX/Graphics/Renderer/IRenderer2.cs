namespace Ssit.CrossX.Graphics.Renderer;

public interface IRenderer2
{
    Size TargetSize { get; }
    Rectangle SafeBounds { get; }
    IStateManager StateManager { get; }
    IRenderStateProvider StateProvider { get; }
    
    IQuadsRenderer QuadsRenderer { get; }
    ISpriteRenderer SpriteRenderer { get; }
    IGeometryRenderer GeometryRenderer { get; }
    ITextRenderer TextRenderer { get; }
    
    void Clear(RgbaColor color);
    void SetRenderTarget(IRenderTarget renderTarget);
}