using System;
using System.IO;
using System.Numerics;
using System.Text;
using Metal;
using Ssit.Pixel.Graphics;
using IFont = Ssit.Pixel.Graphics.IFont;

namespace Ssit.Pixel.NET.Graphics;

internal class RendererImpl: IRenderer
{
    private readonly RenderingDeviceImpl _renderingDevice;
    private readonly IMTLLibrary _defaultLibrary;
    public Size TargetSize => _renderingDevice.TargetSize;
    
    public RendererImpl(RenderingDeviceImpl renderingDevice)
    {
        _renderingDevice = renderingDevice;
        _defaultLibrary =
            renderingDevice.View.Device!.CreateLibrary(LoadShaderSrc("Triangle"), new MTLCompileOptions(), out var _);
    }
    
    private string LoadShaderSrc(string name)
    {
        using var stream = typeof(RenderingDeviceImpl).Assembly.GetManifestResourceStream($"Ssit.Pixel.NET.Shaders.Metal.{name}.metal");
        return new StreamReader(stream!).ReadToEnd();
    }

    public void Clear(RgbaColor color)
    {
        IMTLCommandBuffer commandBuffer = _renderingDevice.CommandBuffer();
        
        // Obtain a renderPassDescriptor generated from the view's drawable textures
        MTLRenderPassDescriptor renderPassDescriptor = _renderingDevice.View.CurrentRenderPassDescriptor;
        
        // If we have a valid drawable, begin the commands to render into it
        if (renderPassDescriptor != null)
        {
            renderPassDescriptor.ColorAttachments[0].ClearColor =
                new MTLClearColor(color.Rf, color.Gf, color.Bf, color.Af);
            renderPassDescriptor.ColorAttachments[0].LoadAction = MTLLoadAction.Clear;
            
            var renderEncoder = commandBuffer.CreateRenderCommandEncoder(renderPassDescriptor);
            renderEncoder.EndEncoding();
        }
    }

    public void DrawText(IFont font, string text, Vector2 position, RgbaColor? color = null)
    {
        throw new NotImplementedException();
    }

    public void DrawText(IFont font, StringBuilder text, Vector2 position, RgbaColor? color = null)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture texture, Rectangle targetRectangle, Rectangle? sourceRectangle = null, IEffect effect = null)
    {
        throw new NotImplementedException();
    }

    public void DrawTexture(ITexture texture, Vector2 position, Rectangle? sourceRectangle = null, Vector2? origin = null,
        float rotation = 0, float scale = 1, RgbaColor? color = null, RenderTransform transform = RenderTransform.None,
        IEffect effect = null)
    {
        throw new NotImplementedException();
    }

    public void FillRectangle(Rectangle rectangle, RgbaColor color)
    {
    }

    public void DrawPrimitives(IVertexBuffer vertexBuffer, ITexture texture = null, RgbaColor? color = null,
        Matrix3x2? transform = null, IEffect effect = null)
    {
        throw new NotImplementedException();
    }
}