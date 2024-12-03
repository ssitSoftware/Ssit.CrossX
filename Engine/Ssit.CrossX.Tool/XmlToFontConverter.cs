using SkiaSharp;
using Ssit.CrossX.Tools;
using Ssit.CrossX.Xml;

namespace Ssit.CrossX.Tool;

internal class XmlToFontConverter(string fullPath, XNode xmlNode) : IXmlFileConverter
{
    public async Task Generate()
    {
        var outputData = Path.Combine(Path.GetDirectoryName(fullPath) ?? "", Path.GetFileNameWithoutExtension(fullPath) ?? "") + $".font";
        var outputBitmap = Path.Combine(Path.GetDirectoryName(fullPath) ?? "", Path.GetFileNameWithoutExtension(fullPath) ?? "") + $".png";
        
        File.Delete(outputData);
        File.Delete(outputBitmap);
        
        using var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Black, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
        await Task.Delay(10);

        float size = 12;
        var font = new SKFont(typeface, size);

        var sk = new SKPaint(font);
        var w = sk.MeasureText("test");
    }
}