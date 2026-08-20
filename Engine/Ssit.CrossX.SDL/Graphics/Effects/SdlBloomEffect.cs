using System.Text;
using SDL;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Effects;
using Ssit.CrossX.SDL.Common;

using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Graphics.Effects;

// ReSharper disable once ClassNeverInstantiated.Global
public unsafe class SdlBloomEffect: IBloomEffect, ISdlGpuEffect
{
    private const string EntryPoint = "fragmentBloom";

    // Only reads the [[position]] built-in from stage_in - it's always populated by the
    // rasterizer regardless of what other varyings (color/texcoord) SDL's own vertex
    // shader emits, so this shader doesn't depend on SDL's internal, undocumented
    // vertex-output attribute layout. UV is reconstructed from screen position using
    // the quad's destination rect, pushed per-draw via PrepareDraw/PushUniforms.
    private const string FragmentSource = """
        #include <metal_stdlib>
        using namespace metal;

        struct FragmentInput
        {
            float4 position [[position]];
        };

        struct BloomUniforms
        {
            float threshold;
            float intensity;
            float destX;
            float destY;
            float destW;
            float destH;
        };

        fragment float4 fragmentBloom(
            FragmentInput in [[stage_in]],
            texture2d<float> tex [[texture(0)]],
            sampler texSampler [[sampler(0)]],
            constant BloomUniforms &uniforms [[buffer(0)]])
        {
            float2 destSize = max(float2(uniforms.destW, uniforms.destH), float2(1.0, 1.0));
            float2 uv = (in.position.xy - float2(uniforms.destX, uniforms.destY)) / destSize;

            float2 texSize = float2(tex.get_width(), tex.get_height());
            float2 texel = 1.0 / max(texSize, float2(1.0, 1.0));

            float4 baseColor = tex.sample(texSampler, uv);

            float2 offsets[8] = {
                float2(-1, -1), float2(0, -1), float2(1, -1),
                float2(-1,  0),                 float2(1,  0),
                float2(-1,  1), float2(0,  1), float2(1,  1)
            };

            float3 bloom = float3(0.0);
            for (int i = 0; i < 8; i++)
            {
                float4 tap = tex.sample(texSampler, uv + offsets[i] * texel * 2.0);
                float luma = dot(tap.rgb, float3(0.299, 0.587, 0.114));
                float contribution = max(luma - uniforms.threshold, 0.0);
                bloom += tap.rgb * contribution;
            }
            bloom *= 0.125;

            float3 result = baseColor.rgb + bloom * uniforms.intensity;
            return float4(result, baseColor.a);
        }
        """;

    private struct BloomUniforms
    {
        public float Threshold;
        public float Intensity;
        public float DestX;
        public float DestY;
        public float DestW;
        public float DestH;
    }

    private readonly SDL_GPUDevice* _device;
    private readonly SDL_GPUShader* _shader;
    private readonly SDL_GPURenderState* _state;
    private bool _disposed;

    private float _threshold = 0.7f;
    private float _intensity = 1.0f;
    private RectangleF _destRect = new(0, 0, 1, 1);

    public float BloomThreshold
    {
        get => _threshold;
        set
        {
            _threshold = value;
            PushUniforms();
        }
    }

    public float BloomIntensity
    {
        get => _intensity;
        set
        {
            _intensity = value;
            PushUniforms();
        }
    }

    public SdlBloomEffect(SdlHandles handles)
    {
        _device = SDL_GetGPURendererDevice(handles.Renderer);
        if (_device is null)
        {
            // Renderer isn't backed by the SDL_GPU API (eg. software/OpenGL drivers) - degrade to a no-op effect.
            return;
        }

        var formats = SDL_GetGPUShaderFormats(_device);
        if ((formats & SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_MSL) == 0)
        {
            // No runtime-compiled shader format available on this backend - degrade to a no-op effect.
            return;
        }

        var codeBytes = Encoding.UTF8.GetBytes(FragmentSource);
        var entryBytes = Encoding.UTF8.GetBytes(EntryPoint + "\0");

        fixed (byte* codePtr = codeBytes)
        fixed (byte* entryPtr = entryBytes)
        {
            var shaderInfo = new SDL_GPUShaderCreateInfo
            {
                code_size = (nuint)codeBytes.Length,
                code = codePtr,
                entrypoint = entryPtr,
                format = SDL_GPUShaderFormat.SDL_GPU_SHADERFORMAT_MSL,
                stage = SDL_GPUShaderStage.SDL_GPU_SHADERSTAGE_FRAGMENT,
                num_samplers = 1,
                num_storage_textures = 0,
                num_storage_buffers = 0,
                num_uniform_buffers = 1
            };

            _shader = SDL_CreateGPUShader(_device, &shaderInfo);
        }

        if (_shader is null)
        {
            return;
        }

        var renderStateInfo = new SDL_GPURenderStateCreateInfo
        {
            fragment_shader = _shader
        };

        _state = SDL_CreateGPURenderState(handles.Renderer, &renderStateInfo);

        if (_state is null)
        {
            SDL_ReleaseGPUShader(_device, _shader);
            _shader = null;
            return;
        }

        PushUniforms();
    }

    private void PushUniforms()
    {
        if (_state is null)
        {
            return;
        }

        var uniforms = new BloomUniforms
        {
            Threshold = _threshold,
            Intensity = _intensity,
            DestX = _destRect.X,
            DestY = _destRect.Y,
            DestW = _destRect.Width,
            DestH = _destRect.Height
        };
        SDL_SetGPURenderStateFragmentUniforms(_state, 0, (IntPtr)(&uniforms), (uint)sizeof(BloomUniforms));
    }

    void ISdlGpuEffect.PrepareDraw(RectangleF destRect)
    {
        _destRect = destRect;
        PushUniforms();
    }

    SDL_GPURenderState* ISdlGpuEffect.GetRenderState(SDL_Renderer* renderer) => _state;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_state != null)
        {
            SDL_DestroyGPURenderState(_state);
        }

        if (_shader != null)
        {
            SDL_ReleaseGPUShader(_device, _shader);
        }

        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
