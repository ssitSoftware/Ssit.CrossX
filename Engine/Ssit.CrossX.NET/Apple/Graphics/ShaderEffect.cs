#if __IOS__ || __MACCATALYST__

using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Metal;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

public abstract class ShaderEffect: IMetalShaderEffect
{
    private readonly IMetalDevice _device;
    public bool OverwriteExistingPixels { get; set; }

    private IMTLFunction _vertexShader;
    private IMTLFunction _fragmentShader;

    private IMTLLibrary _library;
    private IMTLRenderPipelineState _pipelineState;
    
    private int _constBufferSize;

    private IMTLBuffer _constantBuffer;
    
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
        _pipelineState = device.MetalView.Device.CreateRenderPipelineState(pipelineDescriptor, out var error);
    }

    protected void CreateConstantBuffer(int size)
    {
        _constantBuffer = _device.MetalView.Device!.CreateBuffer((uint)size, MTLResourceOptions.CpuCacheModeDefault);
        _constBufferSize = size;
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
        using var stream = typeof(RenderingWindowImpl).Assembly.GetManifestResourceStream($"Ssit.CrossX.NET.Shaders.Metal.{name}.metal");
        return new StreamReader(stream!).ReadToEnd();
    }

    public void Apply(IMTLRenderCommandEncoder encoder, Matrix4x4? world = null)
    {
        encoder.SetRenderPipelineState(_pipelineState);
        
        var transform = Matrix4x4.CreateOrthographicOffCenter(0, _device.TargetSize.Width, _device.TargetSize.Height, 0, 1000, -1000);

        if (world.HasValue)
        {
            transform = Matrix4x4.Multiply(world.Value, transform);
        }

        transform = Matrix4x4.Transpose(transform);
        
        OnApply(encoder, transform);
        
        encoder.SetVertexBuffer(_constantBuffer, 0,  1);
    }

    protected abstract void OnApply(IMTLRenderCommandEncoder encoder, Matrix4x4 transform);

    protected void ApplyConstants<TStruct>(IMTLRenderCommandEncoder encoder, TStruct data) where TStruct : unmanaged
    {
        var size = _constBufferSize;
        unsafe
        {
            void* str = &data;
            {
                void* target = (void*) _constantBuffer.Contents;
                Buffer.MemoryCopy(str, target, size, size);
            }
        }
    }
}

internal class BasicShaderEffectPc : ShaderEffect
{
    public BasicShaderEffectPc(IMetalDevice device)
        : base(device, VertexPositionColor.Mode, "Basic", "vertex_pc", "fragment_pc")
    {
        CreateConstantBuffer(Marshal.SizeOf<Matrix4x4>());
    }

    protected override void OnApply(IMTLRenderCommandEncoder encoder, Matrix4x4 transform)
    {
        ApplyConstants(encoder, transform);
    }
}

internal class BasicShaderEffectPct : ShaderEffect
{
    public BasicShaderEffectPct(IMetalDevice device)
        : base(device, VertexPositionColorTexture.Mode, "BasicTexture", "vertex_pct", "fragment_pct")
    {
        CreateConstantBuffer(Marshal.SizeOf<Matrix4x4>());
    }
    
    protected override void OnApply(IMTLRenderCommandEncoder encoder, Matrix4x4 transform)
    {
        ApplyConstants(encoder, transform);
    }
}

#endif