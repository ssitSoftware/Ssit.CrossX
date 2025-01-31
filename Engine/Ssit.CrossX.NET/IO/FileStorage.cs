using System.IO;
using Microsoft.Maui.Storage;
using Ssit.CrossX.IO;

namespace Ssit.CrossX.NET.IO;

public class FileStorage: IFileStorage
{
    private string GetPath(string path)
    {
        path = Path.Combine(FileSystem.AppDataDirectory, path);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        return path;
    }

    public string ReadText(string path)
    {
        path = GetPath(path);
        return File.ReadAllText(path);
    }

    public void WriteText(string path, string text)
    {
        path = GetPath(path);
        File.WriteAllText(path, text);
    }
}