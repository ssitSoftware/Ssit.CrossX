using System.Text;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics;

/// <summary>
/// IFont interface defines the properties and methods required for handling font details and text measurement.
/// </summary>
public interface IFont
{
    /// <summary>
    /// Gets the name of the font.
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets the size of the font.
    /// </summary>
    int Size { get; }

    /// <summary>
    /// Calculates the size of the specified text.
    /// </summary>
    /// <param name="text">The text to measure.</param>
    /// <returns>A <see cref="Size"/> structure representing the width and height of the text.</returns>
    Size TextSize(string text);

    /// <summary>
    /// Calculates the size of the specified text using a StringBuilder.
    /// </summary>
    /// <param name="text">The text to measure, provided as a StringBuilder instance.</param>
    /// <returns>A <see cref="Size"/> structure representing the width and height of the text.</returns>
    Size TextSize(StringBuilder text);
    
    /// <summary>
    /// Calculates the size of the specified text using an ICharProvider.
    /// </summary>
    /// <param name="text">The text to measure, provided as an ICharProvider instance.</param>
    /// <returns>A <see cref="Size"/> structure representing the width and height of the text.</returns>
    Size TextSize(ICharProvider text);
}