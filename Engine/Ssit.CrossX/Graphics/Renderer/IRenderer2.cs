namespace Ssit.CrossX.Graphics.Renderer;

public interface IRenderer2
{
    Size TargetSize { get; }
    
    IRendererStateManager StateManager { get; }
    
    IQuadsRenderer QuadsRenderer { get; }
    ISpriteRenderer SpriteRenderer { get; }
    IGeometryRenderer GeometryRenderer { get; }
    
    void Clear(RgbaColor color);
    void Flush();
    
    void SetRenderTarget(IRenderTarget renderTarget);
}