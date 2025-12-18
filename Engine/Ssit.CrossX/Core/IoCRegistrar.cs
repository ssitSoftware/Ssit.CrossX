using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.Content;
using Ssit.CrossX.Content.Internal;
using Ssit.CrossX.Core.Internal;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Internal;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.IoC;

namespace Ssit.CrossX.Core;

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
            .WithSingleton<IContentManager, ContentManager>()
            .WithSingleton<IInputMappings, InputMappings>()
            .WithSingleton<IActionScheduler, ActionScheduler>()
            .WithSingleton<ISmartTextRenderer, SmartTextRenderer>();
    }
    
    public static IIoCContainerBuilder WithIndexedRenderer(this IIoCContainerBuilder builder, params RgbaColor[] palette)
    {
        return builder
            .WithSingleton<IIndexedRenderer, IndexedRenderer>()
            .WithInstance<IPaletteSource>(new PaletteSource(palette));
    }
}