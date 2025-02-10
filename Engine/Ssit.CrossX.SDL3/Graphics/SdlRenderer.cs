using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.SDL3.Common;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL3.Graphics;

public unsafe class SdlRenderer: IRenderer2
{
    private readonly SDL_Renderer* _renderer;
    
    public Size TargetSize
    {
        get
        {
            int w, h;
            SDL_GetRenderOutputSize(_renderer, &w, &h);
            return new Size(w, h);
        }
    }
    
    public IStateManager StateManager { get; }
    public IRenderStateProvider StateProvider { get; }
    public IQuadsRenderer QuadsRenderer { get; }
    public ISpriteRenderer SpriteRenderer { get; }
    public IGeometryRenderer GeometryRenderer { get; }
    public ITextRenderer TextRenderer { get; }

    public SdlRenderer(SDL_Renderer* renderer)
    {
        _renderer = renderer;
        
        var stateManager = new StateManager();
        StateManager = stateManager;
        StateProvider = stateManager;
        
        QuadsRenderer = new SdlQuadsRenderer(_renderer, stateManager);
        GeometryRenderer = new SdlGeometryRenderer(_renderer, stateManager);
        SpriteRenderer = new SdlSpriteRenderer(_renderer, stateManager);
        TextRenderer = new TextRenderer(QuadsRenderer);

        stateManager.UpdateBlendMode += UpdateBendMode;
        UpdateBendMode(stateManager.BlendMode);
    }

    private void UpdateBendMode(BlendMode mode)
    {
        SDL_SetRenderDrawBlendMode(_renderer, mode.ToSdlBlendMode());
    }

    public void Clear(RgbaColor color)
    {
        SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDL_RenderClear(_renderer);
    }

    public void SetRenderTarget(IRenderTarget renderTarget)
    {
        if (renderTarget is null)
        {
            SDL_SetRenderTarget(_renderer, null);
            return;
        }
        SDL_SetRenderTarget(_renderer, renderTarget.GetMap<SdlHandle<SDL_Texture>>(TextureMaps.Diffuse).Pointer);
    }
}