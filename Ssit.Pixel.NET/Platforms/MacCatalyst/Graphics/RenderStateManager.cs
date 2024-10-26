using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Metal;
using Microsoft.Maui.Platform;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Graphics.Internal;

namespace Ssit.Pixel.NET.Graphics;

internal class RenderStateManager
{
    private readonly IMetalDevice _metalDevice;
    
    private IRenderTarget _currentRenderTarget;
    private ITexture _currentTexture;
    private IEffect _currentEffect;
    private VertexMode _currentVertexMode = VertexMode.Invalid;
    private Size _currentSize = Size.Zero;
    
    private IMTLRenderCommandEncoder _currentEncoder;
    public IMTLRenderCommandEncoder CurrentEncoder => _currentEncoder;

    private IMetalShaderEffect _basicEffectPC;
    private IMetalShaderEffect _basicEffectPCT;
    
    private IMTLDepthStencilState _depthStencilState;
    
    private readonly int _matrixRawSize = Marshal.SizeOf<Matrix4x4>();
    private readonly byte[] _matrixRawData;
    
    private IMTLBuffer _transformConstantBuffer;
    
    public RenderStateManager(IMetalDevice metalDevice)
    {
        _metalDevice = metalDevice;

        _basicEffectPC = new BasicShaderEffectPc(metalDevice);
        _basicEffectPCT = new BasicShaderEffectPct(metalDevice);
        
        var depthStateDesc = new MTLDepthStencilDescriptor
        {
            DepthCompareFunction = MTLCompareFunction.Less,
            DepthWriteEnabled = true
        };

        _depthStencilState = metalDevice.MetalView.Device!.CreateDepthStencilState(depthStateDesc);
        _matrixRawData = new byte[_matrixRawSize];
        
        _transformConstantBuffer = metalDevice.MetalView.Device.CreateBuffer((uint)_matrixRawSize, MTLResourceOptions.CpuCacheModeDefault);
    }

    public void EndRenderState()
    {
        _currentEncoder?.EndEncoding();
        _currentEncoder = null;
    }
    
    public IMTLRenderCommandEncoder PrepareRenderState(ITexture texture, IEffect effect, VertexMode vertexMode, Action onNewState, RgbaColor? clearColor = null)
    {
        var changeState = clearColor.HasValue;
        changeState |= _currentEncoder is null;
        changeState |= vertexMode != _currentVertexMode;
        changeState |= !ReferenceEquals(effect, _currentEffect);
        changeState |= !ReferenceEquals(texture, _currentTexture);
        changeState |= !ReferenceEquals(_metalDevice.RenderTarget, _currentRenderTarget);
        changeState |= _currentSize != _metalDevice.TargetSize;
        
        if (!changeState)
        {
            return _currentEncoder;
        }

        onNewState();
        EndRenderState();
        
        _currentEffect = effect;
        _currentRenderTarget = _metalDevice.RenderTarget;
        _currentTexture = texture;
        _currentVertexMode = vertexMode;
        _currentSize = _metalDevice.TargetSize;

        var shaderEffect = (effect as IMetalShaderEffect) ?? ((vertexMode & VertexMode.Texture) != 0 ? _basicEffectPCT : _basicEffectPC);
        
        IMTLCommandBuffer commandBuffer = _metalDevice.CommandBuffer;
        
        // Obtain a renderPassDescriptor generated from the view's drawable textures
        MTLRenderPassDescriptor renderPassDescriptor = null;

        if (_metalDevice.RenderTarget is null)
        {
            renderPassDescriptor = _metalDevice.MetalView.CurrentRenderPassDescriptor;
        }
        else
        {
            throw new NotImplementedException();
        }
        
        if (renderPassDescriptor != null)
        {
            renderPassDescriptor.ColorAttachments[0].ClearColor =
                clearColor?.ToMetal() ?? new MTLClearColor(0, 0, 0, 0);

            renderPassDescriptor.ColorAttachments[0].LoadAction =
                clearColor.HasValue ? MTLLoadAction.Clear : MTLLoadAction.Load;
            
            _currentEncoder = commandBuffer.CreateRenderCommandEncoder(renderPassDescriptor);

            shaderEffect.Apply(_currentEncoder);
            
            _currentEncoder.SetDepthStencilState(_depthStencilState);
            
            ApplyTransform();
            return _currentEncoder;
        }

        throw new InvalidOperationException();
    }

    private void ApplyTransform(Matrix3x2? world = null)
    {
        var transform = Matrix4x4.CreateOrthographicOffCenter(0, _metalDevice.TargetSize.Width, _metalDevice.TargetSize.Height, 0, 0, 100);

        if (world.HasValue)
        {
            var m = world.Value;
            var worldTransform = new Matrix4x4(
                m.M11, m.M12, 0, 0, 
                m.M21, m.M22, 0, 0, 
                0, 0, 1, 0, 
                m.M31, m.M32, 0, 1);

            transform = Matrix4x4.Multiply(transform, worldTransform);
        }
        
        transform = Matrix4x4.Transpose(transform);
        
        GCHandle pinnedUniforms = GCHandle.Alloc(transform, GCHandleType.Pinned);
        IntPtr ptr = pinnedUniforms.AddrOfPinnedObject();
        Marshal.Copy(ptr, _matrixRawData, 0, _matrixRawSize);
        pinnedUniforms.Free();

        Marshal.Copy(_matrixRawData, 0, _transformConstantBuffer.Contents, _matrixRawSize);
        _currentEncoder.SetVertexBuffer(_transformConstantBuffer, 0,  1);
    }
}