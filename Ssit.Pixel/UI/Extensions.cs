using Ssit.Pixel.UI.Values;

namespace Ssit.Pixel.UI;

public static class Extensions
{
    public static ISharedValue<string> AsShared(this string str) => new SharedValue<string>(str);
}