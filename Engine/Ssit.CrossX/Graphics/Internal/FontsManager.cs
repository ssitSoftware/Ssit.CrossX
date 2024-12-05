using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Graphics.Internal;

internal class FontsManager: IFontsManager, IDisposable
{
    private class FontsJson
    {
        public string[] Fonts { get; set; }
    }
    
    private readonly IIoCContainer _container;
    private readonly IFilesProvider _filesProvider;

    private readonly Dictionary<string, List<IGlyphFont>> _fonts = new();
    
    public FontsManager(IIoCContainer container, IFilesProvider filesProvider)
    {
        _container = container;
        _filesProvider = filesProvider;
    }
    
    public void Dispose()
    {
        var lists = _fonts.Select(x => x.Value).ToArray();
        foreach (var list in lists)
        {
            list.ForEach(o=>o.Dispose());
        }
        
        _fonts.Clear();
    }

    public void LoadFonts(string fontsJsonPath)
    {
        Dictionary<string, List<IFont>> fonts = new();
        
        using var stream = _filesProvider.Open(fontsJsonPath);
        
        var paths = JsonConvert.DeserializeObject<FontsJson>(new StreamReader(stream).ReadToEnd());
        var dir = Path.GetDirectoryName(fontsJsonPath);

        foreach (var path in paths.Fonts)
        {
            var fontPath = Path.Combine(dir ?? "", path);
            var font = _container.IoCConstruct<GraphicGlyphFont>(fontPath);

            if (!_fonts.TryGetValue(font.Name, out var list))
            {
                list = new List<IGlyphFont>();
                _fonts.Add(font.Name, list);
            }
            list.Add(font);
        }
    }

    public IFont GetFont(string name, int size)
    {
        if (_fonts.TryGetValue(name, out var fonts))
        {
            return fonts.FirstOrDefault(o => o.Size == size);
        }

        return null;
    }
}