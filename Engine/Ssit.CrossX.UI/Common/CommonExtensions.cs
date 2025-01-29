using SampleGame.Services;
using Ssit.CrossX.Common.Services;
using Ssit.CrossX.Common.Views;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.Common;

public static class CommonExtensions
{
    public static IIoCContainerBuilder WithCommonUi( this IIoCContainerBuilder builder )
    {
        return builder
            .WithSingleton<ITranslator, Translator>();
    }
    
    public static IHandlerMapper AddCommonUiMaping( this IHandlerMapper mapper )
    {
        return mapper
            .AddMapping<GameView, GameViewHandler>();
    }
}