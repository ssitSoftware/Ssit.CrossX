using System.Numerics;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics;

public enum BlendMode
{
    None,
    AlphaBlend,
    Additive
}

/// <summary>
/// Provides methods for rendering text, textures, and primitives on the screen.
/// </summary>
public interface IRenderer
{
    RendererStateManager StateManager { get; }
    
    /// <summary>
    /// Provides access to low-level or internal rendering operations that may bypass standard safety checks.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IUnsafeRenderer"/> allowing advanced or experimental rendering functionality.
    /// </value>
    IUnsafeRenderer Unsafe { get; }

    /// <summary>
    /// Gets the current render target that is being used for rendering operations.
    /// </summary>
    /// <value>
    /// An object implementing <see cref="IRenderTarget"/> that represents the active render target.
    /// </value>
    IRenderTarget CurrentRenderTarget { get; }
    
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
    /// Sets the blend mode for rendering operations.
    /// </summary>
    /// <param name="blendMode">The blend mode to use for rendering.</param>
    void SetBlendMode(BlendMode blendMode);
    
    /// <summary>
    /// Clears the render target, setting it to the specified color.
    /// </summary>
    /// <param name="color">The color to use for clearing the render target.</param>
    void Clear(RgbaColor color);
    
    /// <summary>
    /// Draws text on the screen at a specified position.
    /// </summary>
    /// <param name="font">The font to use for rendering the text.</param>
    /// <param name="text">The source of the text to render.</param>
    /// <param name="position">The position on the screen where the text should be drawn.</param>
    /// <param name="align">The alignment of the text. Default is left aligned.</param>
    /// <param name="color">The color of the text. If null, the default color is used.</param>
    /// <param name="spacing">The spacing of the text, determining how condensed or expanded the text appears. Default is normal spacing.</param>
    /// <param name="depth">The depth value used for layering text in 3D space.</param>
    /// <param name="outlineColor">The outline color of the text. If null, no outline is drawn.</param>
    /// /// <param name="context">Rendering context for faster rendering by caching text calculations. Use for rendering multiline texts.</param>
    void DrawText(IFont font, TextSource text, Vector2 position, ContentAlign align = ContentAlign.Left, RgbaColor? color = null, TextSpacing spacing = TextSpacing.Normal, float depth = 0, RgbaColor? outlineColor = null, TextRenderingContext context = null);

    /// <summary>
    /// Draws the specified text on the screen within a defined rectangle.
    /// </summary>
    /// <param name="font">The font to use for rendering the text.</param>
    /// <param name="text">The source of the text to be rendered.</param>
    /// <param name="position">The rectangle that defines the boundaries for the text rendering.</param>
    /// <param name="align">The text alignment within the rectangle. Default is Left.</param>
    /// <param name="color">The color to render the text. If null, a default color is used.</param>
    /// <param name="spacing">The spacing mode to apply between characters. Default is Normal.</param>
    /// <param name="paragraphSpacing">Spacing between paragraphs.</param>
    /// <param name="depth">The depth value to use for rendering order. Default is 0.</param>
    /// <param name="outlineColor">The color to render the text outline. If null, no outline is rendered. Use for rendering multiline texts.</param>
    /// <param name="context">Rendering context for rendering cache.</param>
    void DrawText(IFont font, TextSource text, RectangleF position, ContentAlign align = ContentAlign.Left, RgbaColor? color = null, TextSpacing spacing = TextSpacing.Normal, float paragraphSpacing = -1, float depth = 0, RgbaColor? outlineColor = null, TextRenderingContext context = null);

    /// <summary>
    /// Draws a texture at the specified target rectangle with optional source rectangle and effect.
    /// </summary>
    /// <param name="texture">The texture to be drawn.</param>
    /// <param name="targetRectangle">The destination rectangle on the screen.</param>
    /// <param name="sourceRectangle">The source rectangle from the texture. If null, the entire texture is used.</param>
    /// <param name="color">Color to multiply texture with.</param>
    /// <param name="imageTransform"></param>
    /// <param name="filter">Sampler filter for texture rendering.</param>
    /// <param name="effect">An optional effect to be applied to the texture.</param>
    /// <param name="depth">Z coordinate for drawing - useful in POV perspective and z-buffer based rendering.</param>
    void DrawTexture(ITexture texture, RectangleF targetRectangle, Rectangle? sourceRectangle = null, RgbaColor? color = null, 
        ImageTransform imageTransform = ImageTransform.None,
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
    /// <param name="imageTransform"></param>
    /// <param name="filter">Sampler filter for texture rendering.</param>
    /// <param name="effect">The optional effect to be applied during rendering.</param>
    /// <param name="depth">Z coordinate for drawing - useful in POV perspective and z-buffer based rendering.</param>
    void DrawTexture(ITexture texture, Vector2 position, Rectangle? sourceRectangle = null,
        Vector2? origin = null, float rotation = 0, float scale = 1, RgbaColor? color = null,
        ImageTransform imageTransform = ImageTransform.None,
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
    /// <param name="transform">Transform matrix for this render.</param>
    /// <param name="effect">The optional effect to apply to the rendering. If not provided, no effect will be applied.</param>
    void DrawPrimitives(IVertexBuffer vertexBuffer, int vertexStart, int vertexCount, ITexture texture = null,
        RgbaColor? color = null, TextureFilter filter = TextureFilter.Nearest, Matrix4x4? transform = null, 
        IEffect effect = null);

    void Flush();
}