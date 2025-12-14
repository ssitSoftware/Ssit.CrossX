using Ssit.CrossX.IO;

namespace Ssit.CrossX.Graphics.Font;

public interface IFontSource
{
    IFilesProvider FontsFilesProvider { get; }
    string  FontsDriveName { get; }
}