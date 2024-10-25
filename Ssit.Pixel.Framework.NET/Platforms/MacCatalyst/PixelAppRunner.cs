using SDL2.Bindings;
using Ssit.Pixel.Framework.Core;
using Ssit.Pixel.Framework.Graphics;
using Ssit.Pixel.Framework.NET.Graphics;
using Ssit.Pixel.Framework.NET.Internal;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.NET;

public class PixelAppRunner<TApp>: PixelAppDesktopRunner<TApp> where TApp: PixelApp
{
    protected override SDL.SDL_WindowFlags AdditionalWindowFlags => SDL.SDL_WindowFlags.SDL_WINDOW_METAL;

    private PixelAppRunner()
    {
    }

    public static void Run(WindowParameters windowParameters = null, IIoCContainerBuilder builder = null)
    {
        var runner = new PixelAppRunner<TApp>();
        runner.RunInternal(windowParameters, builder);
    }

    protected override void InitializeRenderer(IntPtr windowHandle, IIoCContainerBuilder builder)
    {
        base.InitializeRenderer(windowHandle, builder);
        
        var renderingDevice = new RenderingDeviceImpl(SdlRendererHandle);
        
        builder
            .WithInstance<IRenderingDevice>(renderingDevice)
            .WithInstance(renderingDevice.Renderer);
    }
}