using System;
using System.IO;
using System.Text;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.Text;

namespace Ssit.CrossX.Graphics.Internal;

internal class GraphicGlyphFont : GlyphFont, IFont, IDisposable
{
    private ITexture FontSheet { get; }

    public GraphicGlyphFont(string path, IFilesProvider filesProvider, IIoCContainer iocContainer)
    {
        using var stream = filesProvider.Open(path);
        Load(stream);

        var sheetPath = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileNameWithoutExtension(path)) + ".png";
        
        if (filesProvider.FileExists(sheetPath))
        {
            using var textStream = filesProvider.Open(sheetPath);
            FontSheet = iocContainer.IoCConstruct<ITexture>(new LoadTextureParameters
            {
                DiffuseMapStream = textStream
            });
        }
    }
    
    public void Dispose()
    {
        FontSheet?.Dispose();
    }
    
    public Size TextSize(string text) => TextSize(new TextSource
    {
        String = text
    });

    public Size TextSize(StringBuilder text) => TextSize(new TextSource
    {
        Builder = text
    });

    public Size TextSize(ICharProvider text) => TextSize(new TextSource
    {
        Provider = text
    });
}