using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Internal;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI;

public static class UiBootstrapper
{
    public static IUiApp InitializeUi(this IIoCContainer container, InitializeUiDelegate init)
    {
        var map = new NavigationMap();
        
        var builder = IoC.IoC.NewBuilder();
        builder.WithParent(container);

        builder
            .WithInstance(map)
            .WithSingleton<INavigation, Navigation>()
            .WithSingleton<IStylesManager, StylesManager>()
            .WithSingleton<IHandlerMapper, HandlerMapper>()
            .WithSingleton<IUiServices, UiServices>()
            .WithSingleton<IUiApp, UiApp>();
        
        init(map, builder);
        
        var services = builder.Build();
        var navigation = services.Get<INavigation>();

        var handlerMapper = services.Get<IHandlerMapper>();
        MapHandlers(handlerMapper);
        
        var app = (UiApp)services.Get<IUiApp>();
        app.Navigation = (Navigation)navigation;

        return app;
    }

    private static void MapHandlers(IHandlerMapper handlerMapper)
    {
        handlerMapper
            .AddMapping<Container, ContainerHandler>()
            .AddMapping<Background, BackgroundHandler>();
    }
}