using Ssit.Pixel.Framework.Core;
using Ssit.Pixel.Framework.IoC;
using Ssit.Pixel.NET.Internal;

namespace Ssit.Pixel.Framework.NET;

// All the code in this file is only included on Mac Catalyst.
public class PixelAppRunner<TApp>: PixelAppDesktopRunner<TApp> where TApp: PixelApp
{
    protected PixelAppRunner()
    {
    }

    public static void Run(IIoCContainerBuilder builder = null)
    {
        var runner = new PixelAppRunner<TApp>();
        runner.RunInternal(null, builder);
    }
}