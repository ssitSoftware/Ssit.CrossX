namespace Ssit.CrossX.Graphics.Font;

public enum ScaleMode
{
    None,
    Integer,
    Float
}

/// <summary>
/// Service managing fonts in the application.
/// </summary>
public interface IFontsManager
{
    /// <summary>
    /// Loads font definitions from a JSON stream and initializes the fonts' collection.
    /// </summary>
    /// <param name="fontsJsonPath">A path to the JSON data describing the fonts.</param>
    void LoadFonts(string fontsJsonPath);

    /// <summary>
    /// Retrieves a font with the specified name and size from the font manager.
    /// </summary>
    /// <param name="name">The name of the font to retrieve.</param>
    /// <param name="size">The size of the font to retrieve.</param>
    /// <param name="scaleMode">Mode for scaling</param>
    /// <returns>Pair of an <see cref="IFont"/> instance corresponding to the specified name and size, or null if not found and scaling factor.</returns>
    IFont GetFont(string name, int size);
}