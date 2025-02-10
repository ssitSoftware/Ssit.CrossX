using System.IO;

namespace Ssit.CrossX.IO;

public class FilesStorage(string storageDirectory) : IFileStorage
{
    public string ReadText(string path)
    {
        return File.ReadAllText(Path.Combine(storageDirectory, path));
    }

    public void WriteText(string path, string text)
    {
        File.WriteAllText(Path.Combine(storageDirectory, path), text);
    }
}