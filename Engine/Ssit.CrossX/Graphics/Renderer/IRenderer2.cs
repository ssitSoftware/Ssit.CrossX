namespace Ssit.CrossX.Graphics.Renderer;

public interface IRenderer2
{
    Size TargetSize { get; }
    
    IStateManager StateManager { get; }
    
    IQuadsRenderer QuadsRenderer { get; }
    ISpriteRenderer SpriteRenderer { get; }
    IGeometryRenderer GeometryRenderer { get; }
    
    ITextRenderer TextRenderer { get; }
    
    void Clear(RgbaColor color);
    void SetRenderTarget(IRenderTarget renderTarget);
}