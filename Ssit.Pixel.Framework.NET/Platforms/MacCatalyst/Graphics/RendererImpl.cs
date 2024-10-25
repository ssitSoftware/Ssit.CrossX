using System.Numerics;
using System.Text;
using Ssit.Pixel.Framework.Graphics;
using IFont = Ssit.Pixel.Framework.Graphics.IFont;

using static SDL2.Bindings.SDL;

namespace Ssit.Pixel.Framework.NET.Graphics;

internal class RendererImpl: IRenderer
{
    private readonly IntPtr _rendererHandle;
    private readonly RenderingDeviceImpl _renderingDevice;

    public Size TargetSize
    {
        get
        {
            SDL_GetRendererOutputSize(_rendererHandle, out var w, out var h);
            return new Size(w, h);
        }
    }
    
    public RendererImpl(IntPtr rendererHandle, RenderingDeviceImpl renderingDevice)
    {
        _rendererHandle = rendererHandle;
        _renderingDevice = renderingDevice;
    }

    public void Clear(RgbaColor color)
    {
        SetRenderColor(color);
        SDL_RenderClear(_rendererHandle);
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
        const float scale = 1f;
        
        var sdlRect = new SDL_Rect
        {
            x = (int)(rectangle.X * scale),
            y = (int)(rectangle.Y * scale),
            w = (int)(rectangle.Width * scale),
            h = (int)(rectangle.Height * scale)
        };
        
        SetRenderColor(color);
        SDL_RenderFillRect(_rendererHandle, ref sdlRect);
    }

    public void DrawPrimitives(IVertexBuffer vertexBuffer, ITexture texture = null, RgbaColor? color = null,
        Matrix3x2? transform = null, IEffect effect = null)
    {
        throw new NotImplementedException();
    }
    
    private void SetRenderColor(RgbaColor color) => SDL_SetRenderDrawColor(_rendererHandle, color.R, color.G, color.B, color.A);
}