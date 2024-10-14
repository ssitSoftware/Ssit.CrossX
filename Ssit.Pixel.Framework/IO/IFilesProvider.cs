using System.IO;

namespace Ssit.Pixel.Framework.IO;

/// <summary>
/// Provides methods to open files from various locations.
/// </summary>
public interface IFilesProvider
{
    /// <summary>
    /// Opens a file stream for the given path.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <returns>A stream to read the file.</returns>
    Stream Open(string path);
}