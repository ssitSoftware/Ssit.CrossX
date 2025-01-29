namespace Ssit.CrossX.Games.Template;

public class MaterialInfo
{
    public MaterialInfo(string name, string shortName, RgbaColor previewColor)
    {
        Name = name;
        ShortName = shortName;
        PreviewColor = previewColor;
    }

    public string Name { get; }
    public string ShortName { get; }
    public RgbaColor PreviewColor { get; }
}