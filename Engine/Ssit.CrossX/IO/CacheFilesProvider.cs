using System;
using System.IO;
using System.Reflection;

namespace Ssit.CrossX.IO;

public class CacheFilesProvider: IFilesProvider
{
    private readonly string _dir;
    
    public CacheFilesProvider(Assembly assembly, string dir, string name)
    {
        var filesProvider = new EmbeddedFilesProvider(assembly, dir ?? assembly.GetName().Name);
        var files =  filesProvider.GetFiles("");
        
        _dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
        Directory.CreateDirectory(_dir);
        
        foreach (var file in files)
        {
            var fname = Path.GetFileName(file);
            var targetFile = Path.Combine(_dir, fname);

            if (File.Exists(targetFile))
            {
                var time = File.GetLastWriteTime(file);
                var targetTime = File.GetLastWriteTime(targetFile);

                if (time <= targetTime) continue;
            }

            using var stream = filesProvider.Open(file);
            using var fileStream = File.OpenWrite(targetFile);
            
            stream.CopyTo(fileStream);
            stream.Flush();
            fileStream.Flush();
        }
    }
    
    public Stream Open(string path)
    {
        return File.Open(Path.Combine(_dir, path), FileMode.Open);
    }

    public bool FileExists(string path)
    {
        return File.Exists(Path.Combine(_dir, path));
    }

    public string[] GetFiles(string path, string extension = null)
    {
        var ext = extension ?? "*";
        return Directory.GetFiles(Path.Combine(_dir, path), $"*.{ext}");
    }

    public string GetPhisicalFilePath(string path)
    {
        return Path.Combine(_dir, path);
    }
}