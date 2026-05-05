namespace Ssit.CrossX.IO;

public interface IAssetsSource
{
    IFilesProvider FilesProvider { get; }
    string DriveName { get; }
    string DefinitionPath => DriveName + "/Fonts.json";
}