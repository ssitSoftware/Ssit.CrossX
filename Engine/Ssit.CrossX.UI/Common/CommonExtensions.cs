using Ssit.CrossX.Common.Pages;
using Ssit.CrossX.Common.Services;
using Ssit.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.Common;

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