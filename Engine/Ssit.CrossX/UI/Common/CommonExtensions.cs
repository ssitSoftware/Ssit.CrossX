using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Common.Services;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;
using Ssit.IoC;

namespace Ssit.CrossX.UI.Common;

public static class CommonExtensions
{
    public static IIoCContainerBuilder WithCommonUi( this IIoCContainerBuilder builder )
    {
        return builder
            .WithSingleton<ITranslator, Translator>()
            .WithSingleton<PageInputContext, PageInputContext>();
    }
    
    public static IHandlerMapper AddCommonUiMaping( this IHandlerMapper mapper )
    {
        return mapper
            .AddMapping<GameView, GameViewHandler>();
    }
}