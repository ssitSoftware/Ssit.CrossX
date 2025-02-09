using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.Content;
using Ssit.CrossX.Content.Internal;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Internal;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.IoC;

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
            .WithSingleton<IMusicPlayer, MusicPlayer>()
            .WithSingleton<IInputMappings, InputMappings>()
            .WithSingleton<IActionScheduler, ActionScheduler>();
    }
}