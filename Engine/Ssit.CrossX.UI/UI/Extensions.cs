using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI;

public static class Extensions
{
    public static TView ApplyStyles<TView>(this TView view, IStylesManager styles, params string[] styleNames) where TView : View
    {
        return view;
    }
}