using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Internal;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.SDL.Common;
using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL.Graphics;

public unsafe class SdlRenderer: IRenderer2
{
    private readonly SDL_Renderer* _renderer;
    private readonly SdlQuadsRenderer _quadsRenderer;
    private readonly SdlSpriteRenderer _spriteRenderer;
    private readonly SdlGeometryRenderer _geometryRenderer;

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

    public IQuadsRenderer QuadsRenderer => _quadsRenderer;

    public ISpriteRenderer SpriteRenderer => _spriteRenderer;

    public IGeometryRenderer GeometryRenderer => _geometryRenderer;

    public ITextRenderer TextRenderer { get; }

    internal bool DisableRendering;

    public SdlRenderer(SDL_Renderer* renderer)
    {
        _renderer = renderer;
        
        var stateManager = new StateManager();
        StateManager = stateManager;
        StateProvider = stateManager;
        
        _quadsRenderer = new SdlQuadsRenderer(_renderer, stateManager);
        _geometryRenderer = new SdlGeometryRenderer(_renderer, stateManager);
        _spriteRenderer = new SdlSpriteRenderer(_renderer, stateManager);
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

    public void ResetStats()
    {
        _quadsRenderer.ResetStats();
        _geometryRenderer.ResetStats();
        _spriteRenderer.ResetStats();
    }
}