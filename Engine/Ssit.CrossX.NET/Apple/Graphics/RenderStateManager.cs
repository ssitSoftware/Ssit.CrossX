#if __IOS__ || __MACCATALYST__

using System;
using System.Numerics;
using Metal;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.NET.Apple.Graphics;

internal class RenderStateManager: IDisposable
{
    private readonly IMetalDevice _metalDevice;
    
    private IRenderTarget _currentRenderTarget;
    private ITexture _currentTexture;
    private IEffect _currentEffect;
    private BlendMode _currentBlendMode = BlendMode.None;
    private RgbaColor _currentBlendColor;
    
    private TextureFilter _currentTextureFilter = TextureFilter.Nearest;
    
    private VertexMode _currentVertexMode = VertexMode.Invalid;
    private Size _currentSize = Size.Zero;
    
    private IMTLRenderCommandEncoder _currentEncoder;
    public IMTLRenderCommandEncoder CurrentEncoder => _currentEncoder;

    private IMetalShaderEffect _basicEffectPC;
    private IMetalShaderEffect _basicEffectPCT;
    
    private IMTLDepthStencilState _depthStencilState;

    private IMTLSamplerState _nearestSamplerState, _linearSamplerState;

    private MTLRenderPassDescriptor _offscreenDescriptor = new()
    {
        DepthAttachment = new MTLRenderPassDepthAttachmentDescriptor(),
        StencilAttachment = new MTLRenderPassStencilAttachmentDescriptor()
    };

    

    public RenderStateManager(IMetalDevice metalDevice)
    {
        _metalDevice = metalDevice;

        _basicEffectPC = new BasicShaderEffectPc(metalDevice);
        _basicEffectPCT = new BasicShaderEffectPct(metalDevice);
        
        var depthStateDesc = new MTLDepthStencilDescriptor
        {
            DepthCompareFunction = MTLCompareFunction.LessEqual,
            DepthWriteEnabled = true
        };

        _depthStencilState = metalDevice.MetalView.Device!.CreateDepthStencilState(depthStateDesc);

        _nearestSamplerState = metalDevice.MetalView.Device.CreateSamplerState(new MTLSamplerDescriptor
        {
            MagFilter = MTLSamplerMinMagFilter.Nearest,
            MinFilter = MTLSamplerMinMagFilter.Nearest,
            BorderColor = MTLSamplerBorderColor.OpaqueBlack,
        });
        
        _linearSamplerState = metalDevice.MetalView.Device.CreateSamplerState(new MTLSamplerDescriptor
        {
            MagFilter = MTLSamplerMinMagFilter.Linear,
            MinFilter = MTLSamplerMinMagFilter.Linear,
            BorderColor = MTLSamplerBorderColor.OpaqueBlack,
        });
    }

    public void EndRenderState()
    {
        _currentEncoder?.EndEncoding();
        _currentEncoder = null;
    }
    
    public IMTLRenderCommandEncoder PrepareRenderState(ITexture texture, IEffect effect, VertexMode vertexMode,
        TextureFilter filter,
        BlendMode blendMode,
        Action onNewState, 
        Matrix4x4? worldTransform,
        RgbaColor blendColor,
        RgbaColor? clearColor = null)
    {
        var changeState = clearColor.HasValue;
        changeState |= _currentEncoder is null;
        changeState |= vertexMode != _currentVertexMode;
        changeState |= !ReferenceEquals(effect, _currentEffect);
        changeState |= !ReferenceEquals(texture, _currentTexture);
        changeState |= !ReferenceEquals(_metalDevice.CurrentRenderTarget, _currentRenderTarget);
        changeState |= _currentSize != _metalDevice.TargetSize;
        changeState |= _currentTextureFilter != filter;
        changeState |= blendMode != _currentBlendMode;
        changeState |= blendColor != _currentBlendColor;
        
        if (!changeState)
        {
            return _currentEncoder;
        }

        onNewState();
        EndRenderState();
        
        _currentEffect = effect;
        _currentRenderTarget = _metalDevice.CurrentRenderTarget;
        _currentTexture = texture;
        _currentVertexMode = vertexMode;
        _currentSize = _metalDevice.TargetSize;
        _currentTextureFilter = filter;
        _currentBlendMode = blendMode;
        _currentBlendColor = blendColor;

        var shaderEffect = (effect as IMetalShaderEffect) ?? ((vertexMode & VertexMode.Texture) != 0 ? _basicEffectPCT : _basicEffectPC);
        
        IMTLCommandBuffer commandBuffer = _metalDevice.CommandBuffer;

        if (commandBuffer is null)
        {
            throw new InvalidOperationException();
        }
        
        // Obtain a renderPassDescriptor generated from the view's drawable textures
        MTLRenderPassDescriptor renderPassDescriptor;

        if (_metalDevice.CurrentRenderTarget is null)
        {
            renderPassDescriptor = _metalDevice.MetalView.CurrentRenderPassDescriptor;
        }
        else
        {
            renderPassDescriptor = _offscreenDescriptor;
            
            renderPassDescriptor.ColorAttachments[0].Texture =
                _metalDevice.CurrentRenderTarget.GetMap<IMTLTexture>(TextureMaps.Diffuse);
            
            renderPassDescriptor.DepthAttachment!.Texture =
                _metalDevice.CurrentRenderTarget.GetMap<IMTLTexture>(TextureMaps.DepthBuffer);
            
            renderPassDescriptor.StencilAttachment!.Texture =
                _metalDevice.CurrentRenderTarget.GetMap<IMTLTexture>(TextureMaps.DepthBuffer);
            
            renderPassDescriptor.RenderTargetWidth = (uint)_metalDevice.CurrentRenderTarget.Size.Width;
            renderPassDescriptor.RenderTargetHeight = (uint)_metalDevice.CurrentRenderTarget.Size.Height;
        }
        
        if (renderPassDescriptor != null)
        {
            renderPassDescriptor.ColorAttachments[0].ClearColor =
                clearColor?.ToMetal() ?? new MTLClearColor(0, 0, 0, 0);

            renderPassDescriptor.ColorAttachments[0].LoadAction =
                clearColor.HasValue ? MTLLoadAction.Clear : MTLLoadAction.Load;
            
            renderPassDescriptor.DepthAttachment!.LoadAction = clearColor.HasValue ? MTLLoadAction.Clear : MTLLoadAction.Load;
            renderPassDescriptor.StencilAttachment!.LoadAction = clearColor.HasValue ? MTLLoadAction.Clear : MTLLoadAction.Load;
            
            _currentEncoder = commandBuffer.CreateRenderCommandEncoder(renderPassDescriptor);

            shaderEffect.Apply(_currentEncoder, worldTransform, blendColor);
            
            _currentEncoder.SetDepthStencilState(_depthStencilState);

            if (texture is not null)
            {
                _currentEncoder.SetFragmentSamplerState(_currentTextureFilter == TextureFilter.Nearest ? _nearestSamplerState : _linearSamplerState, 0);
                _currentEncoder.SetFragmentTexture(texture.GetMap<IMTLTexture>(TextureMaps.Diffuse), 0);
            }
            
            return _currentEncoder;
        }

        throw new InvalidOperationException();
    }

    public void Dispose()
    {
        _currentEncoder?.EndEncoding();
        
        _basicEffectPC?.Dispose();
        _basicEffectPCT?.Dispose();
        _depthStencilState?.Dispose();
        _nearestSamplerState?.Dispose();
        _linearSamplerState?.Dispose();
    }
}

#endif