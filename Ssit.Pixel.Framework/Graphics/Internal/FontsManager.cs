using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.Graphics.Internal;

internal class FontsManager: IFontsManager, IDisposable
{
    private readonly IIoCContainer _container;

    private class FontsDesc
    {
        public Font.FontInfo[] Fonts { get; set; }
    }
    
    private readonly Dictionary<string, Font> _fonts = new();
    
    private Font _default;

    public IFont this[string name] => _fonts.GetValueOrDefault(name, _default);
    
    public FontsManager(IIoCContainer container)
    {
        _container = container;
    }
    
    public void LoadFonts(Stream jsonStream)
    {
        var data = new StreamReader(jsonStream).ReadToEnd();
        var fontsDesc = JsonConvert.DeserializeObject<FontsDesc>(data);
        
        foreach (var fontInfo in fontsDesc.Fonts)
        {
            var font = _container.IoCConstruct<Font>(fontInfo);
            _fonts.Add(fontInfo.Name, font);
            
            _default ??= font;
        }
    }
    
    public void Dispose()
    {
        foreach (var font in _fonts)
        {
            font.Value.Dispose();
        }
        _fonts.Clear();
    }
}