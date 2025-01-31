namespace Ssit.CrossX.IO;

public interface IFileStorage
{
    string ReadText(string path);
    void WriteText(string path, string text);
}