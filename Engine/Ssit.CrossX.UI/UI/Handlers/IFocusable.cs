using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Handlers;

public interface IFocusable
{
    bool Enabled { get; }
    bool OnUiButton(UiButton button, IInputContext inputContext);
    void SetFocus();
    bool ResetFocus();
    string UniqueId { get; }
}