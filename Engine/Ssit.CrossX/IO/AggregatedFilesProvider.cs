using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ssit.CrossX.IO;

public class AggregatedFilesProvider: IFilesProvider
{
    private readonly Dictionary<string, IFilesProvider> _fileProviders = new();

    public AggregatedFilesProvider(AggregatedFilesProvider other = null)
    {
        if (other != null)
        {
            foreach (var kv in other._fileProviders)
            {
                _fileProviders.Add(kv.Key, kv.Value);
            }
        }
    }
    
    public AggregatedFilesProvider AddProvider(string drive, IFilesProvider provider)
    {
        _fileProviders.Add(drive, provider);
        return this;
    }
    
    public Stream Open(string path)
    {
        PathHelper.GetDriveAndPath(path, out var drive, out var newPath);

        if (!_fileProviders.TryGetValue(drive, out var provider))
        {
            throw new FileNotFoundException($"File {path} not found. Drive not found.");
        }

        return provider.Open(newPath);
    }

    public bool FileExists(string path)
    {
        PathHelper.GetDriveAndPath(path, out var drive, out var newPath);

        if (!_fileProviders.TryGetValue(drive, out var provider))
        {
            return false;
        }
        
        return provider.FileExists(newPath);
    }

    public string[] GetFiles(string path, string extension)
    {
        PathHelper.GetDriveAndPath(path, out var drive, out var newPath);
        
        if (!_fileProviders.TryGetValue(drive, out var provider))
        {
            return [];
        }

        return provider.GetFiles(newPath, extension).Select(o => drive + '/' + o).ToArray();
    }

    public string GetPhisicalFilePath(string path)
    {
        PathHelper.GetDriveAndPath(path, out var drive, out var newPath);
        
        if (!_fileProviders.TryGetValue(drive, out var provider))
        {
            throw new FileNotFoundException($"File {path} not found. Drive not found.");
        }
        
        return provider.GetPhisicalFilePath(newPath);
    }
}