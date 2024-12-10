using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Handlers;

namespace Ssit.CrossX.UI.Services;

public interface IUiServices
{
    public IIoCContainer IoCContainer { get; }
    public IStylesManager StylesManager { get; }
    public IHandlerMapper HandlerMapper { get; }
}