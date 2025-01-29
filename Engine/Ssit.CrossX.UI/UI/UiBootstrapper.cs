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
        var handlers = new HandlerMapper();
        
        var builder = IoC.IoC.NewBuilder();
        builder.WithParent(container);

        builder
            .WithInstance(map)
            .WithSingleton<INavigation, Navigation>()
            .WithSingleton<IStylesManager, StylesManager>()
            .WithSingleton<IHandlerMapper, FullHandlerMapper>()
            .WithSingleton<IUiServices, UiServices>()
            .WithSingleton<IActionDispatcher, ActionDispatcher>()
            .WithSingleton<IUiApp, UiApp>();
        
        init(builder, map, handlers);
        
        var services = builder.Build();
        var navigation = services.Get<INavigation>();

        var handlerMapper = (FullHandlerMapper)services.Get<IHandlerMapper>();
        MapHandlers(handlerMapper);
        handlerMapper.AddMappings(handlers);
        
        var app = (UiApp)services.Get<IUiApp>();
        app.Initialize((Navigation)navigation);
        
        return app;
    }

    private static void MapHandlers(IHandlerMapper handlerMapper)
    {
        handlerMapper
            .AddMapping<Container, ContainerHandler>()
            .AddMapping<Background, BackgroundHandler>()
            .AddMapping<Label, LabelHandler<Label>>()
            .AddMapping<LabelButton, LabelButtonHandler<LabelButton>>()
            .AddMapping<TextView, TextViewHandler>()
            .AddMapping<Button, ButtonHandler>()
            .AddMapping<VerticalStack, VerticalStackHandler<VerticalStack>>()
            .AddMapping<ImageView, ImageViewHandler>()
            .AddMapping<ScrollView, ScrollViewHandler<ScrollView>>();
    }
}