using System;
using System.IO;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.IO;
using Ssit.IoC;
using Ssit.CrossX.Text;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.Graphics.Internal;

// ReSharper disable once ClassNeverInstantiated.Global
internal class GraphicGlyphFont : GlyphFont, IGlyphFont
{
    public ITexture OutlineSheet { get; }
    public ITexture FontSheet { get; }

    public int LineSize => Metrics.LineHeight;
    
    public GraphicGlyphFont(string path, IFilesProvider filesProvider, IIoCContainer iocContainer)
    {
        using var stream = filesProvider.Open(path);
        Load(stream);

        var sheetPath = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileNameWithoutExtension(path)) + ".png";
        (FontSheet, OutlineSheet) = TextureHelper.LoadComplexSheet(filesProvider, iocContainer, sheetPath);
    }

    public void Dispose() => FontSheet?.Dispose();

    public Size TextSize(TextSource text, TextSpacing spacing) => GlyphFontRenderer.MeasureText(this, text, spacing);
}