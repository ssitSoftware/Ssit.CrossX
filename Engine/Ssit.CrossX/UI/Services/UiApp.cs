using Ssit.CrossX.Graphics.Renderer;
using Ssit.IoC;
using Ssit.CrossX.UI.Exceptions;

namespace Ssit.CrossX.UI.Services;

internal class UiApp(IIoCContainer services, IActionDispatcher iActionDispatcher)
    : IUiApp
{
    INavigation IUiApp.Navigation => Navigation;
    
    public Navigation Navigation { get; private set; }
    public RectangleF Bounds { get; private set; }

    public float Scale { get; private set; }

    public IIoCContainer Services { get; private set; } = services;
    public InputProcessor InputProcessor { get; private set; }
    
    private readonly ActionDispatcher _actionDispatcher = (ActionDispatcher)iActionDispatcher;

    public void Initialize(Navigation navigation)
    {
        InputProcessor = Services.IoCConstruct<InputProcessor>(navigation);
        Navigation = navigation;
    }
    
    public void Update(float dt)
    {
        _actionDispatcher.Dispatch();

        InputProcessor.Process();
        Navigation.Update(dt);
    }

    public void Draw(IRenderer2 renderer,RgbaColor? clearColor = null)
    {
        for (var idx = 0; idx < 8; ++idx)
        {
            try
            {
                InternalDraw(renderer, clearColor);
                break;
            }
            catch (InvalidRenderingException)
            {
                Navigation.CurrentPage?.RecalculateLayout();
                Navigation.CurrentPage?.Update(0);
                
                _actionDispatcher.Dispatch();
                Navigation.Update(0);
            }
        }
    }

    private void InternalDraw(IRenderer2 renderer, RgbaColor? clearColor = null)
    {
        if (renderer.StateProvider.UseGlowTextures)
        {
            renderer.Clear(RgbaColor.Black);
        }
        else if(clearColor?.A > 0)
        {
            renderer.Clear(clearColor.Value);
        }
        
        if (!Navigation.PreviousPageOnTop && Navigation.PreviousPage is not null)
        {
            Navigation.PreviousPage.Draw(renderer);
        }

        if (Navigation.CurrentPage is not null)
        {
            if (Navigation.ParallelTransitions || Navigation.PreviousPage is null)
            {
                Navigation.CurrentPage.Draw(renderer);
            }
        }

        if (Navigation.PreviousPageOnTop && Navigation.PreviousPage is not null)
        {
            Navigation.PreviousPage.Draw(renderer);
        }
    }

    public void SetBounds(RectangleF bounds, float scale)
    {
        Bounds = bounds;
        Scale = scale;
        
        Navigation.PreviousPage?.SetBounds(bounds, scale);
        Navigation.CurrentPage?.SetBounds(bounds, scale);
    }

    public void Dispose()
    {
        var services = Services;
        Services = null;
        services?.Dispose();
    }
}