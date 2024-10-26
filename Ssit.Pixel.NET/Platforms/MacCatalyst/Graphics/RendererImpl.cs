using System;
using System.IO;
using System.Numerics;
using Metal;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Graphics.Internal;

namespace Ssit.Pixel.NET.Graphics;

internal class RendererImpl: RendererBase
{
    private readonly RenderingDeviceImpl _renderingDevice;
    private readonly RenderStateManager _renderStateManager;

    private readonly IMTLBuffer _mtlColorVertexBuffer;
    private readonly IMTLBuffer _mtlTextureVertexBuffer;
    
    public override Size TargetSize => _renderingDevice.TargetSize;
    
    public RendererImpl(RenderingDeviceImpl renderingDevice)
    {
        _renderingDevice = renderingDevice;
        _renderStateManager = new RenderStateManager(_renderingDevice);

        _mtlColorVertexBuffer =
            renderingDevice.View.Device.CreateBuffer( (UIntPtr)(ColorVertexBuffer.Size * ColorVertexBuffer.Stride),
                MTLResourceOptions.StorageModeShared);
        
        _mtlTextureVertexBuffer =
            renderingDevice.View.Device.CreateBuffer( (UIntPtr)(TextureVertexBuffer.Size * TextureVertexBuffer.Stride),
                MTLResourceOptions.StorageModeShared);
    }

    public override void Clear(RgbaColor color)
    {
        _renderStateManager.PrepareRenderState(null, null, VertexMode.Invalid, TextureFilter.Nearest, Flush, color);
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
            _renderingDevice.CommitCommandBuffer();
        }
    }

    protected override void PrepareRendering(ITexture texture, IEffect effect, VertexMode vertexMode, TextureFilter textureFilter) 
        => _renderStateManager.PrepareRenderState(texture, effect, vertexMode, textureFilter, Flush);

    private void DrawColorBuffer()
    {
        ColorVertexBuffer.CopyDataTo(_mtlColorVertexBuffer.Contents, (int)_mtlColorVertexBuffer.Length);

        var encoder = _renderStateManager.CurrentEncoder;
        
        encoder.SetVertexBuffer(_mtlColorVertexBuffer, 0, 0);
        encoder.DrawPrimitives(MTLPrimitiveType.Triangle, 0, (UIntPtr)ColorVertexBuffer.Offset);
    }

    private void DrawTextureBuffer()
    {
        TextureVertexBuffer.CopyDataTo(_mtlTextureVertexBuffer.Contents, (int)_mtlTextureVertexBuffer.Length);

        var encoder = _renderStateManager.CurrentEncoder;
        
        encoder.SetVertexBuffer(_mtlTextureVertexBuffer, 0, 0);
        encoder.DrawPrimitives(MTLPrimitiveType.Triangle, 0, (UIntPtr)TextureVertexBuffer.Offset);
    }

    public override void DrawPrimitives(IVertexBuffer vertexBuffer,int vertexStart, int vertexCount, ITexture texture = null, RgbaColor? color = null,
        Matrix3x2? transform = null, TextureFilter textureFilter = TextureFilter.Nearest, IEffect effect = null)
    {
        if (CurrentBatchMode != BatchMode.None)
        {
            Flush();
        }

        CurrentBatchMode = BatchMode.None;
        
        var mtlBuffer = vertexBuffer.Get<IMTLBuffer>();
        var encoder = _renderStateManager.PrepareRenderState(texture, effect, vertexBuffer.VertexMode, textureFilter, Flush);
        
        encoder.SetVertexBuffer(mtlBuffer, 0, 0);
        encoder.DrawPrimitives(vertexBuffer.PrimitiveType.ToMetal(), (UIntPtr)vertexStart, (UIntPtr)vertexCount);
    }
}