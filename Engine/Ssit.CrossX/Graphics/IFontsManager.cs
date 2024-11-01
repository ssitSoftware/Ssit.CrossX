using System.IO;

namespace Ssit.CrossX.Graphics;

/// <summary>
/// Service managing fonts in the application.
/// </summary>
public interface IFontsManager
{
    /// <summary>
    /// Loads font definitions from a JSON stream and initializes the fonts' collection.
    /// </summary>
    /// <param name="jsonStream">A stream containing the JSON data describing the fonts.</param>
    void LoadFonts(Stream jsonStream);

    /// <summary>
    /// Gets font with a given name or default.
    /// </summary>
    IFont this[string name] { get; }
}