using Ssit.CrossX.UI.Common.Services;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;
using Ssit.IoC;

namespace Ssit.CrossX.UI.Common;

public static class CommonExtensions
{
    public static IIoCContainerBuilder WithTranslator(this IIoCContainerBuilder builder, string languagesPath)
    {
        return builder
            .WithSingleton<ITranslator, Translator>(languagesPath);
    }
    
    public static IIoCContainerBuilder WithTranslator(this IIoCContainerBuilder builder, params string[] languagesPaths)
    {
        return builder
            .WithSingleton<ITranslator, Translator>(languagesPaths);
    }
    
    public static IHandlerMapper AddCommonUiMaping(this IHandlerMapper mapper)
    {
        return mapper
            .AddMapping<GameView, GameViewHandler>();
    }
}