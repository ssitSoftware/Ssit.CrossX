using System.Numerics;
using Ssit.CrossX.IO;

namespace Ssit.CrossX.XxFormats.Template;

public interface IGameTemplate
{
    Vector2 Gravity { get; }
    string Name { get; }
    Guid Guid { get; }
    int TileSize { get; }
    
    RgbaColor DefaultBackground { get; }
    
    LayerDescription[] Layers { get; }
    ObjectDescription[] Objects { get; }
    ImageDescription[] Images { get; }

    string[] TileSets { get; }
    OriginAlignment ObjectsOriginAlignment { get; }
    
    MaterialInfo[] Materials { get; }
    IFilesProvider AssetsProvider { get; }
    decimal TilesetPanelZoom { get; }
    int PreviewZoom { get; }
    RgbaColor EmptyColor { get; }
    RgbaColor TilesBgColor => EmptyColor;
    int TrimToPixels => 0;
    
    Size TargetSize { get; }
}