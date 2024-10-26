using System.IO;
using System.Numerics;
using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

public class ShaderEffect: IMetalShaderEffect
{
    private readonly IMetalDevice _device;
    public bool OverwriteExistingPixels { get; set; }

    private IMTLFunction _vertexShader;
    private IMTLFunction _fragmentShader;

    private IMTLLibrary _library;
    private IMTLRenderPipelineState _pipelineState;
    
    public ShaderEffect(IMetalDevice device, VertexMode vertexMode, string path, string vsName, string fsName)
    {
        _device = device;
        
        _library =
            device.MetalView.Device!.CreateLibrary(LoadShaderSrc(path), new MTLCompileOptions(), out var _);
        
        _vertexShader = _library.CreateFunction(vsName);
        _fragmentShader = _library.CreateFunction(fsName);

        var vertexDescriptor = vertexMode.GetVertexDescriptor();
        
        var pipelineDescriptor = new MTLRenderPipelineDescriptor
        {
            SampleCount = device.MetalView.SampleCount,
            VertexFunction = _vertexShader,
            FragmentFunction = _fragmentShader,
            VertexDescriptor = vertexDescriptor,
            DepthAttachmentPixelFormat = device.MetalView.DepthStencilPixelFormat,
            StencilAttachmentPixelFormat = device.MetalView.DepthStencilPixelFormat
        };

        pipelineDescriptor.ColorAttachments[0].PixelFormat = device.MetalView.ColorPixelFormat;
        _pipelineState = device.MetalView.Device.CreateRenderPipelineState(pipelineDescriptor, out var _);
    }
    
    public void Dispose()
    {
        _pipelineState?.Dispose();
        _vertexShader?.Dispose();
        _fragmentShader?.Dispose();
        _library?.Dispose();

        _pipelineState = null;
        _vertexShader = null;
        _fragmentShader = null;
        _library = null;
    }
    
    private string LoadShaderSrc(string name)
    {
        using var stream = typeof(RenderingDeviceImpl).Assembly.GetManifestResourceStream($"Ssit.Pixel.NET.Shaders.Metal.{name}.metal");
        return new StreamReader(stream!).ReadToEnd();
    }

    public virtual void Apply(IMTLRenderCommandEncoder encoder)
    {
        encoder.SetRenderPipelineState(_pipelineState);
    }
}

internal class BasicShaderEffectPc(IMetalDevice device)
    : ShaderEffect(device, VertexPositionColor.Mode, "Basic", "vertex_pc", "fragment_pc");

internal class BasicShaderEffectPct(IMetalDevice device)
    : ShaderEffect(device, VertexPositionColorTexture.Mode, "Basic", "vertex_pc", "fragment_pc");