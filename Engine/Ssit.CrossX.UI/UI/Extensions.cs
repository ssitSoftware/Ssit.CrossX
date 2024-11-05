using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI;

public static class Extensions
{
    public static ISharedValue<string> AsShared(this string str) => new SharedValue<string>(str);

    public static TView ApplyStyles<TView>(this TView view, IStylesManager styles, params string[] styleNames) where TView : View
    {
        return view;
    }
}