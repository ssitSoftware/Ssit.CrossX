using System.Numerics;
using System.Text;

namespace Ssit.CrossX.Graphics;

/// <summary>
/// Provides methods for rendering text, textures, and primitives on the screen.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Sets the render target for subsequent rendering operations.
    /// </summary>
    /// <param name="renderTarget">The render target to set. If null, the app window render target will be used.</param>
    void SetRenderTarget(IRenderTarget renderTarget);
    
    /// <summary>
    /// Gets the target size of the renderer.
    /// </summary>
    /// <value>
    /// A <see cref="Size"/> structure representing the width and height
    /// of the rendering target.
    /// </value>
    Size TargetSize { get; }

    /// <summary>
    /// Sets the transformation matrix that will be used for rendering.
    /// </summary>
    /// <param name="matrix">The transformation matrix to apply. If null, the identity matrix will be used.</param>
    void SetTransform(Matrix3x2? matrix);
    
    /// <summary>
    /// Sets the transformation matrix that will be used for rendering.
    /// </summary>
    /// <param name="matrix">The transformation matrix to apply.</param>
    void SetTransform(Matrix4x4 matrix);
    
    /// <summary>
    /// Clears the render target, setting it to the specified color.
    /// </summary>
    /// <param name="color">The color to use for clearing the render target.</param>
    void Clear(RgbaColor color);
    
    /// <summary>
    /// Renders the specified text string at a given position with an optional color.
    /// </summary>
    /// <param name="font">The font used to render the text.</param>
    /// <param name="text">The text string to render.</param>
    /// <param name="position">The screen coordinates where the text should be rendered.</param>
    /// <param name="color">The optional color of the text. If not provided, a default color will be used.</param>
    /// <param name="depth">Z coordinate for drawing - useful in POV perspective and z-buffer based rendering.</param>
    void DrawText(IFont font, string text, Vector2 position, RgbaColor? color = null, float depth = 0);
    
    /// <summary>
    /// Renders the specified text string at a given position with an optional color.
    /// </summary>
    /// <param name="font">The font used to render the text.</param>
    /// <param name="text">A StringBuilder containing the text string to render.</param>
    /// <param name="position">The screen coordinates where the text should be rendered.</param>
    /// <param name="color">The optional color of the text. If not provided, a default color will be used.</param>
    /// <param name="depth">Z coordinate for drawing - useful in POV perspective and z-buffer based rendering.</param>
    void DrawText(IFont font, StringBuilder text, Vector2 position, RgbaColor? color = null, float depth = 0);

    /// <summary>
    /// Draws a texture at the specified target rectangle with optional source rectangle and effect.
    /// </summary>
    /// <param name="texture">The texture to be drawn.</param>
    /// <param name="targetRectangle">The destination rectangle on the screen.</param>
    /// <param name="sourceRectangle">The source rectangle from the texture. If null, the entire texture is used.</param>
    /// <param name="color">Color to multiply texture with.</param>
    /// <param name="filter">Sampler filter for texture rendering.</param>
    /// <param name="effect">An optional effect to be applied to the texture.</param>
    /// <param name="depth">Z coordinate for drawing - useful in POV perspective and z-buffer based rendering.</param>
    void DrawTexture(ITexture texture, Rectangle targetRectangle, Rectangle? sourceRectangle = null, RgbaColor? color = null, 
        TextureFilter filter = TextureFilter.Nearest, 
        IEffect effect = null, float depth = 0);
    
    /// <summary>
    /// Draws the specified texture at a given position with optional parameters for the source rectangle, origin, rotation, scale, color, and effect.
    /// </summary>
    /// <param name="texture">The texture to be drawn.</param>
    /// <param name="position">The screen coordinates where the texture should be rendered.</param>
    /// <param name="sourceRectangle">An optional source rectangle to specify the portion of the texture to draw.</param>
    /// <param name="origin">An optional origin point for rotation and scaling.</param>
    /// <param name="rotation">The optional rotation angle in radians.</param>
    /// <param name="scale">The optional scale factor for rendering the texture.</param>
    /// <param name="color">The optional color to apply to the texture. If not provided, the default color will be used.</param>
    /// <param name="filter">Sampler filter for texture rendering.</param>
    /// <param name="effect">The optional effect to be applied during rendering.</param>
    /// <param name="depth">Z coordinate for drawing - useful in POV perspective and z-buffer based rendering.</param>
    void DrawTexture(ITexture texture, Vector2 position, Rectangle? sourceRectangle = null,
        Vector2? origin = null, float rotation = 0, float scale = 1, RgbaColor? color = null,
        TextureFilter filter = TextureFilter.Nearest,
        IEffect effect = null, float depth = 0);

    /// <summary>
    /// Fills a rectangle on the screen with the specified color.
    /// </summary>
    /// <param name="rectangle">The rectangle to fill, defined by its position and size.</param>
    /// <param name="color">The color to use for filling the rectangle.</param>
    /// <param name="depth">Z coordinate for drawing - useful in POV perspective and z-buffer based rendering.</param>
    void FillRectangle(RectangleF rectangle, RgbaColor color, float depth = 0);

    /// <summary>
    /// Renders a set of primitives using the provided texture and vertex buffer, with optional color, transformation, and effect.
    /// </summary>
    /// <param name="vertexBuffer">The vertex buffer containing the vertices that represent the primitives.</param>
    /// <param name="vertexStart">Specifies index of first vertex to draw.</param>
    /// <param name="vertexCount">Specifies number of vertices to draw.</param>
    /// <param name="texture">The texture to apply to the primitives. If not provided, no texture will be used.</param>
    /// <param name="color">The optional color to apply to the primitives. If not provided, a default color will be used.</param>
    /// <param name="filter">Sampler filter for texture rendering.</param>
    /// <param name="effect">The optional effect to apply to the rendering. If not provided, no effect will be applied.</param>
    void DrawPrimitives(IVertexBuffer vertexBuffer, int vertexStart, int vertexCount, ITexture texture = null,
        RgbaColor? color = null, TextureFilter filter = TextureFilter.Nearest, 
        IEffect effect = null);

    void Flush();
}