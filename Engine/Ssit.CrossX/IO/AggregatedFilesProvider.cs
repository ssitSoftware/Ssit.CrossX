using System.Collections.Generic;
using System.IO;

namespace Ssit.CrossX.IO;

public class AggregatedFilesProvider: IFilesProvider
{
    private Dictionary<string, IFilesProvider> _fileProviders = new();

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
}