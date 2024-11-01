using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI;

public interface IStylesManager
{

}

public static class Extensions
{
    public static ISharedValue<string> AsShared(this string str) => new SharedValue<string>(str);

    public static TView ApplyStyles<TView>(this TView view, IStylesManager styles, params string[] styleNames) where TView : View
    {
        return view;
    }
}