using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.UI.Services;

internal class UiApp(IIoCContainer services, IActionDispatcher iActionDispatcher)
    : IUiApp, IUiAppBoundsSource
{
    INavigation IUiApp.Navigation => Navigation;
    
    public Navigation Navigation { get; set; }
    public RectangleF Bounds { get; private set; }
    
    public IIoCContainer Services { get; private set; } = services;
    
    private readonly ActionDispatcher _iActionDispatcher = (ActionDispatcher)iActionDispatcher;
    
    public void Update(float dt)
    {
        _iActionDispatcher.Dispatch();
        Navigation.Update(dt);
    }

    public void Draw(IRenderer renderer, RgbaColor? clearColor = null)
    {
        if (clearColor?.A > 0)
        {
            renderer.Clear(clearColor.Value);
        }
        
        if (!Navigation.PreviousPageOnTop && Navigation.PreviousPage is not null)
        {
            Navigation.PreviousPage.Draw(renderer);
        }
        
        if (Navigation.CurrentPage is not null)
        {
            Navigation.CurrentPage.Draw(renderer);
        }
        
        if (Navigation.PreviousPageOnTop && Navigation.PreviousPage is not null)
        {
            Navigation.PreviousPage.Draw(renderer);
        }
    }

    public void SetBounds(RectangleF bounds)
    {
        Bounds = bounds;
        Navigation.PreviousPage?.SetBounds(bounds);
        Navigation.CurrentPage?.SetBounds(bounds);
    }

    public void Dispose()
    {
        var services = Services;
        Services = null;
        services?.Dispose();
    }
}