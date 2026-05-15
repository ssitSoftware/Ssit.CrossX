using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Handlers;

public interface IUiCommandHandler
{
    bool OnUiButton(UiButton button, IInputContext context);
}