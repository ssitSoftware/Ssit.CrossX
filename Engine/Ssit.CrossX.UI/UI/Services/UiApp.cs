using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;

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
    
    private readonly ActionDispatcher _iActionDispatcher = (ActionDispatcher)iActionDispatcher;

    public void Initialize(Navigation navigation)
    {
        InputProcessor = Services.IoCConstruct<InputProcessor>(navigation);
        Navigation = navigation;
    }
    
    public void Update(float dt)
    {
        _iActionDispatcher.Dispatch();

        InputProcessor.Process();
        Navigation.Update(dt);
    }

    public void Draw(IRenderer renderer, RenderMode mode, RgbaColor? clearColor = null)
    {
        if (clearColor?.A > 0 && mode == RenderMode.Normal)
        {
            renderer.Clear(clearColor.Value);
        }
        
        if (!Navigation.PreviousPageOnTop && Navigation.PreviousPage is not null && mode != RenderMode.Debug)
        {
            Navigation.PreviousPage.Draw(renderer, mode);
        }
        
        if (Navigation.CurrentPage is not null)
        {
            Navigation.CurrentPage.Draw(renderer, mode);
        }
        
        if (Navigation.PreviousPageOnTop && Navigation.PreviousPage is not null && mode != RenderMode.Debug)
        {
            Navigation.PreviousPage.Draw(renderer, mode);
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