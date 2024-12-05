using System;
using System.IO;
using System.Text;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.Text;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.Graphics.Internal;

// ReSharper disable once ClassNeverInstantiated.Global
internal class GraphicGlyphFont : GlyphFont, IGlyphFont
{
    public ITexture OutlineSheet { get; }
    public ITexture FontSheet { get; }

    public GraphicGlyphFont(string path, IFilesProvider filesProvider, IIoCContainer iocContainer)
    {
        using var stream = filesProvider.Open(path);
        Load(stream);

        var sheetPath = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileNameWithoutExtension(path)) + ".png";

        (FontSheet, OutlineSheet) = LoadComplexSheet(filesProvider, iocContainer, sheetPath);
    }

    private (ITexture, ITexture) LoadComplexSheet(IFilesProvider filesProvider, IIoCContainer container, string path)
    {
        using var stream = filesProvider.Open(path);
        var colors = ImagesUtility.LoadImage(stream);
        
        var fillColors = new RgbaColor[colors.GetLength(0), colors.GetLength(1)];
        var outlineColors = new RgbaColor[colors.GetLength(0), colors.GetLength(1)];

        for (var x = 0; x < colors.GetLength(0); x++)
        {
            for (var y = 0; y < colors.GetLength(1); y++)
            {
                var color = colors[x, y];
                if (color.A > 0)
                {
                    var outlineAlpha = color.A;
                    var fillAlpha = color.G;

                    fillColors[x, y] = new RgbaColor(255, 255, 255, fillAlpha);
                    outlineColors[x, y] = new RgbaColor(255, 255, 255, outlineAlpha);
                }
            }
        }

        ITexture outlineSheet;
        using (var outlineStream = ImagesUtility.GetStream(outlineColors))
        {
            outlineSheet = container.IoCConstruct<ITexture>(new LoadTextureParameters
            {
                DiffuseMapStream = outlineStream
            });
        }
        
        ITexture fillSheet;
        using (var fillStream = ImagesUtility.GetStream(fillColors))
        {
            fillSheet = container.IoCConstruct<ITexture>(new LoadTextureParameters
            {
                DiffuseMapStream = fillStream
            });
        }
        
        return (fillSheet, outlineSheet);
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