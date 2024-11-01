using System.IO;

namespace Ssit.CrossX.IO;

public class BundleFilesProvider: IFilesProvider
{
    private readonly string _bundleDir = "";
    public BundleFilesProvider()
    {
        var location = GetType().Assembly.Location;
        var dir = Path.GetDirectoryName(location) ?? string.Empty;

        string[] dirs = [
            dir + "/Resources/",
            dir + "/../Resources/"
        ];

        foreach (var directory in dirs)
        {
            var testDir = PathHelper.NormalizePath(directory);
            if (Directory.Exists(testDir))
            {
                _bundleDir = testDir;
                break;
            }
        }
    }
    
    public Stream Open(string path)
    {
        var resPath = GetFullPath(path);
        return File.Open(resPath, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public bool FileExists(string path)
    {
        path = GetFullPath(path);
        return FileExists(path);
    }

    private string GetFullPath(string path)
    {
        path = PathHelper.NormalizePath(path);
        path = Path.Combine(_bundleDir, path);

        return path;
    }
}