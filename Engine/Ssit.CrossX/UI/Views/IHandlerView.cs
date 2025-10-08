using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI.Views;

internal interface IHandlerView
{
    ViewHandler Handler { get; }
    void Initialize(IUiServices services);
}