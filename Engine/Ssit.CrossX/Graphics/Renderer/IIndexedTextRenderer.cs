using System.Numerics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics.Renderer;

public interface IIndexedTextRenderer
{
    /// <summary>
    /// Draws text on the screen at a specified position.
    /// </summary>
    /// <param name="font">The font to use for rendering the text.</param>
    /// <param name="text">The source of the text to render.</param>
    /// <param name="position">The position on the screen where the text should be drawn.</param>
    /// <param name="align">The alignment of the text. Default is left aligned.</param>
    /// <param name="color">The color of the text. If null, the default color is used.</param>
    /// <param name="spacing">The spacing of the text, determining how condensed or expanded the text appears. Default is normal spacing.</param>
    /// <param name="outlineColor">The outline color of the text. If null, no outline is drawn.</param>
    /// <param name="lineSpacing">The spacing between text lines. Default is 0.</param>
    /// <param name="context">Rendering context for faster rendering by caching text calculations. Use for rendering multiline texts.</param>
    void DrawText(IFont font, TextSource text, Vector2 position, byte color, ContentAlign align = ContentAlign.Left, 
        TextSpacing spacing = TextSpacing.Normal, byte? outlineColor = null, int lineSpacing = 0, TextRenderingContext context = null);

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
    /// <param name="outlineColor">The color to render the text outline. If null, no outline is rendered. Use for rendering multiline texts.</param>
    /// <param name="lineSpacing">The spacing between text lines. Default is 0.</param>
    /// <param name="context">Rendering context for rendering cache.</param>
    void DrawText(IFont font, TextSource text, RectangleF position, byte color, ContentAlign align = ContentAlign.Left, 
        TextSpacing spacing = TextSpacing.Normal, float paragraphSpacing = -1, byte? outlineColor = null, int lineSpacing = 0,
        TextRenderingContext context = null);
}