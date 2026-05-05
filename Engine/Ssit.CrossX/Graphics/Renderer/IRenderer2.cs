namespace Ssit.CrossX.Graphics.Renderer;

public interface IRenderer2
{
    bool EnableSafeBounds { set; }
    Size TargetSize { get; }
    Rectangle Bounds { get; }
    IStateManager StateManager { get; }
    IRenderStateProvider StateProvider { get; }
    
    IQuadsRenderer QuadsRenderer { get; }
    ISpriteRenderer SpriteRenderer { get; }
    IGeometryRenderer GeometryRenderer { get; }
    ITextRenderer TextRenderer { get; }
    
    void Clear(RgbaColor color);
    void SetRenderTarget(IRenderTarget renderTarget);
}