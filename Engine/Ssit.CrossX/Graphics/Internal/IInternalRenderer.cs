namespace Ssit.CrossX.Graphics.Internal;

public interface IInternalRenderer
{
    void BeginRender(ITexture texture, TextureFilter textureFilter);
    void FastDrawTexture(ITexture texture, Rectangle target, Rectangle source, RgbaColor color, float depth = 0);
}