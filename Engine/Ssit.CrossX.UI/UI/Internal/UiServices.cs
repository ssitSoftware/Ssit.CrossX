using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI.Internal;

public class UiServices: IUiServices
{
    public UiServices(IIoCContainer ioCContainer, IStylesManager stylesManager, IHandlerMapper handlerMapper, IRenderModeProvider renderModeProvider)
    {
        IoCContainer = ioCContainer;
        StylesManager = stylesManager;
        HandlerMapper = handlerMapper;
        RenderModeProvider = renderModeProvider;
    }

    public IIoCContainer IoCContainer { get; }
    public IStylesManager StylesManager { get; }
    public IHandlerMapper HandlerMapper { get; }
    public IRenderModeProvider RenderModeProvider { get; }
}