using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI.Internal;

public class UiServices: IUiServices
{
    public UiServices(IIoCContainer ioCContainer, IStylesManager stylesManager, IHandlerMapper handlerMapper)
    {
        IoCContainer = ioCContainer;
        StylesManager = stylesManager;
        HandlerMapper = handlerMapper;
    }

    public IIoCContainer IoCContainer { get; }
    public IStylesManager StylesManager { get; }
    public IHandlerMapper HandlerMapper { get; }
}