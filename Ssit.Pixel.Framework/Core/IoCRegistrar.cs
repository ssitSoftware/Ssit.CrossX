using Ssit.Pixel.Framework.Content;
using Ssit.Pixel.Framework.Content.Internal;
using Ssit.Pixel.Framework.Graphics;
using Ssit.Pixel.Framework.Graphics.Internal;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.Core;

internal static class IoCRegistrar
{
    /// <summary>
    /// Registers the essential Pixel Core components with the IoC Container Builder.
    /// </summary>
    /// <param name="builder">The IoC container builder used for registering dependencies.</param>
    /// <returns>The IoC container builder with registered Pixel Core components.</returns>
    public static IIoCContainerBuilder WithPixelCore(this IIoCContainerBuilder builder)
    {
        return builder
            .WithSingleton<IFontsManager, FontsManager>()
            .WithSingleton<IContentManager, ContentManager>();
    }
}