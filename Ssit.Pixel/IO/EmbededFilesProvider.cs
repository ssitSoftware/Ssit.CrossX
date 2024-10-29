using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Ssit.Pixel.IO;

public class EmbededFilesProvider: IFilesProvider
{
    private Assembly _assembly;
    private HashSet<string> _files;
    private string _prefix;

    public EmbededFilesProvider(Assembly assembly, string prefix = null)
    {
        _prefix = (prefix ?? _assembly.GetName().Name) + '.';
        _assembly = assembly;
        _files = new HashSet<string>(_assembly.GetManifestResourceNames()); 
    }
    
    private string GetResourceName(string path)
    {
        path = PathHelper.NormalizePath(path).Replace('/', '.');
        path = _prefix + path;
        return path;
    }
    
    public Stream Open(string path)
    {
        path = GetResourceName(path);

        if (!_files.Contains(path))
        {
            throw new FileNotFoundException($"Resource {path} not found in assembly {_assembly.GetName().Name}.");
        }
        
        var stream = _assembly.GetManifestResourceStream(path);
        return stream ?? throw new InvalidProgramException();
    }

    public bool FileExists(string path)
    {
        path = GetResourceName(path);
        return _files.Contains(path);
    }
}