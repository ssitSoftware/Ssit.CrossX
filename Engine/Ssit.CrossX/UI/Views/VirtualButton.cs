using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class VirtualButton: Container
{
    public GameControllerButton Button { get; set; }
    public ColorWrapper ColorPressed { get; set; }
    public ColorWrapper OutlineColorPressed { get; set; }
    public SharedValue<bool> HapticFeedback { get; set; } = false;
}