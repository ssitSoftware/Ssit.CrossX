using System;
using Metal;
using Ssit.Pixel.Graphics;

namespace Ssit.Pixel.NET.Graphics;

internal class RenderStateManager
{
    private readonly IMetalDevice _metalDevice;
    
    private IRenderTarget _currentRenderTarget;
    private ITexture _currentTexture;
    private IEffect _currentEffect;
    private TextureFilter _currentTextureFilter = TextureFilter.Nearest;
    
    private VertexMode _currentVertexMode = VertexMode.Invalid;
    private Size _currentSize = Size.Zero;
    
    private IMTLRenderCommandEncoder _currentEncoder;
    public IMTLRenderCommandEncoder CurrentEncoder => _currentEncoder;

    private IMetalShaderEffect _basicEffectPC;
    private IMetalShaderEffect _basicEffectPCT;
    
    private IMTLDepthStencilState _depthStencilState;

    private IMTLSamplerState _nearestSamplerState, _linearSamplerState;
    
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
    
    public IMTLRenderCommandEncoder PrepareRenderState(ITexture texture, IEffect effect, VertexMode vertexMode, TextureFilter filter,
        Action onNewState, RgbaColor? clearColor = null)
    {
        var changeState = clearColor.HasValue;
        changeState |= _currentEncoder is null;
        changeState |= vertexMode != _currentVertexMode;
        changeState |= !ReferenceEquals(effect, _currentEffect);
        changeState |= !ReferenceEquals(texture, _currentTexture);
        changeState |= !ReferenceEquals(_metalDevice.RenderTarget, _currentRenderTarget);
        changeState |= _currentSize != _metalDevice.TargetSize;
        changeState |= _currentTextureFilter != filter;
        
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
        _currentTextureFilter = filter;

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

            if (texture is not null)
            {
                _currentEncoder.SetFragmentSamplerState(_currentTextureFilter == TextureFilter.Nearest ? _nearestSamplerState : _linearSamplerState, 0);
                _currentEncoder.SetFragmentTexture(texture.GetMap<IMTLTexture>(TextureMaps.Diffuse), 0);
            }
            
            return _currentEncoder;
        }

        throw new InvalidOperationException();
    }
}