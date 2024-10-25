using Ssit.Pixel.Content;
using Ssit.Pixel.Content.Internal;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Graphics.Internal;
using Ssit.Pixel.IoC;

namespace Ssit.Pixel.Core;

public static class IoCRegistrar
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