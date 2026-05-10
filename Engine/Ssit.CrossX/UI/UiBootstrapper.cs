using Ssit.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Handlers.Markdown;
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
            .WithSingleton<IUiSounds, UiSoundsContainer>()
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
            .AddMapping<Frame, FrameHandler>()
            .AddMapping<Label, LabelHandler<Label>>()
            .AddMapping<BlinkingLabel, BlinkingLabelHandler<BlinkingLabel>>()
            .AddMapping<LabelButton, LabelButtonHandler<LabelButton>>()
            .AddMapping<LabelButtonEx, LabelButtonExHandler>()
            .AddMapping<LabelRadio, LabelRadioHandler<LabelRadio>>()
            .AddMapping<Button, ButtonHandler>()
            .AddMapping<VerticalStack, VerticalStackHandler<VerticalStack>>()
            .AddMapping<ImageView, ImageViewHandler>()
            .AddMapping<ScrollView, ScrollViewHandler<ScrollView>>()
            .AddMapping<VirtualButton, VirtualButtonHandler>()
            .AddMapping<IconCheckBox, IconCheckBoxHandler<IconCheckBox>>()
            .AddMapping<SpriteView, SpriteViewHandler>()
            .AddMapping<MarkdownView, MarkdownViewHandler<MarkdownView>>();
    }
}