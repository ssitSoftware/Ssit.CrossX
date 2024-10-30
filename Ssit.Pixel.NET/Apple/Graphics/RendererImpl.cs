#if __IOS__ || __MACCATALYST__

using System;
using Metal;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Graphics.Internal;

namespace Ssit.Pixel.NET.Apple.Graphics;

internal class RendererImpl: RendererBase, IDisposable
{
    private readonly IMetalDevice _metalDevice;
    private readonly RenderStateManager _renderStateManager;

    private readonly IMTLBuffer _mtlColorVertexBuffer;
    private readonly IMTLBuffer _mtlTextureVertexBuffer;
    
    public override Size TargetSize => _metalDevice.TargetSize;
    
    public RendererImpl(IMetalDevice metalDevice)
    {
        _metalDevice = metalDevice;
        _renderStateManager = new RenderStateManager(_metalDevice);
        
        _mtlColorVertexBuffer =
            metalDevice.MetalView.Device!.CreateBuffer( (UIntPtr)(ColorVertexBuffer.Size * ColorVertexBuffer.Stride),
                MTLResourceOptions.StorageModeShared);
        
        _mtlTextureVertexBuffer =
            metalDevice.MetalView.Device.CreateBuffer( (UIntPtr)(TextureVertexBuffer.Size * TextureVertexBuffer.Stride),
                MTLResourceOptions.StorageModeShared);
    }

    public override void Clear(RgbaColor color)
    {
        _renderStateManager.PrepareRenderState(null, null, VertexMode.Invalid, TextureFilter.Nearest, Flush, WorldTransform, color);
        Flush();
    }

    public override void SetRenderTarget(IRenderTarget renderTarget)
    {
        _metalDevice.CurrentRenderTarget = renderTarget;
    }

    public override void Flush()
    {
        bool encoderWasUsed = _renderStateManager.CurrentEncoder is not null;
        
        if (encoderWasUsed)
        {
            if (CurrentBatchMode == BatchMode.TextureBuffer)
            {
                DrawTextureBuffer();
            }
            else if (CurrentBatchMode == BatchMode.ColorBuffer)
            {
                DrawColorBuffer();
            }
        }
        
        TextureVertexBuffer.Reset();
        ColorVertexBuffer.Reset();
        
        CurrentBatchMode = BatchMode.None;

        if (encoderWasUsed)
        {
            _renderStateManager.EndRenderState();
            _metalDevice.CommitCommandBuffer();
        }
    }

    protected override void PrepareRendering(ITexture texture, IEffect effect, VertexMode vertexMode, TextureFilter textureFilter) 
        => _renderStateManager.PrepareRenderState(texture, effect, vertexMode, textureFilter, Flush, WorldTransform);

    private void DrawColorBuffer()
    {
        ColorVertexBuffer.CopyDataTo(_mtlColorVertexBuffer.Contents, (int)_mtlColorVertexBuffer.Length);

        var encoder = _renderStateManager.CurrentEncoder;
        
        encoder.SetVertexBuffer(_mtlColorVertexBuffer, (UIntPtr)0, (UIntPtr)0);
        encoder.DrawPrimitives(MTLPrimitiveType.Triangle, (UIntPtr)0, (UIntPtr)ColorVertexBuffer.Offset);
    }

    private void DrawTextureBuffer()
    {
        TextureVertexBuffer.CopyDataTo(_mtlTextureVertexBuffer.Contents, (int)_mtlTextureVertexBuffer.Length);

        var encoder = _renderStateManager.CurrentEncoder;
        
        encoder.SetVertexBuffer(_mtlTextureVertexBuffer, (UIntPtr)0, (UIntPtr)0);
        encoder.DrawPrimitives(MTLPrimitiveType.Triangle, (UIntPtr)0, (UIntPtr)TextureVertexBuffer.Offset);
    }

    public override void DrawPrimitives(IVertexBuffer vertexBuffer,int vertexStart, int vertexCount, ITexture texture = null, RgbaColor? color = null,
        TextureFilter textureFilter = TextureFilter.Nearest, IEffect effect = null)
    {
        if (CurrentBatchMode != BatchMode.None)
        {
            Flush();
        }

        CurrentBatchMode = BatchMode.None;
        
        var mtlBuffer = vertexBuffer.Get<IMTLBuffer>();
        var encoder = _renderStateManager.PrepareRenderState(texture, effect, vertexBuffer.VertexMode, textureFilter, Flush, WorldTransform);
        
        encoder.SetVertexBuffer(mtlBuffer, 0, 0);
        encoder.DrawPrimitives(vertexBuffer.PrimitiveType.ToMetal(), (UIntPtr)vertexStart, (UIntPtr)vertexCount);
    }

    public void Dispose()
    {
        _mtlColorVertexBuffer?.Dispose();
        _mtlTextureVertexBuffer?.Dispose();
        _renderStateManager?.Dispose();
    }
}

#endif