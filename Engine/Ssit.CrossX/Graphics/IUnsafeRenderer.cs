namespace Ssit.CrossX.Graphics;

public interface IUnsafeRenderer
{
    void BeginRender(ITexture texture, TextureFilter textureFilter);
    void FastDrawQuad(ITexture texture, Rectangle target, Rectangle source, RgbaColor color, float depth = 0);
}