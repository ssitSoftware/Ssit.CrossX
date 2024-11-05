using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI.Internal;

public class UiServices: IUiServices
{
    public UiServices(IIoCContainer ioCContainer, IStylesManager stylesManager)
    {
        IoCContainer = ioCContainer;
        StylesManager = stylesManager;
    }

    public IIoCContainer IoCContainer { get; }
    public IStylesManager StylesManager { get; }
}