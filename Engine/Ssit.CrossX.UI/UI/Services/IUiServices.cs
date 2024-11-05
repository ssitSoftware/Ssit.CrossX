using Ssit.CrossX.IoC;

namespace Ssit.CrossX.UI.Services;

public interface IUiServices
{
    public IIoCContainer IoCContainer { get; }
    public IStylesManager StylesManager { get; }
}