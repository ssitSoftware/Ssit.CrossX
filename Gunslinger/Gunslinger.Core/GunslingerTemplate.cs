using System;
using Ssit.CrossX;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.IO;

namespace Gunslinger.Core;

public class GunslingerTemplate: IGameTemplate
{
    public string Name => "Gunslinger";
    public Guid Guid { get; } = Guid.Parse("b7d05cc4-a3f3-461f-8cb2-07ec58b6120a");

    public IFilesProvider AssetsProvider { get; } = new AggregatedFilesProvider()
        .AddProvider("assets:", 
            new EmbeddedFilesProvider(typeof(GunslingerTemplate).Assembly, "Gunslinger.Core.Assets"));
    
    public int TileSize => 16;
    public int TargetWidth => 640;
    public int TargetHeight => 360;
    
    public RgbaColor DefaultBackground => new(0xff404040);
    public RgbaColor EmptyColor => new(128, 128, 128);
    
    public GameObject.OriginAlignment ObjectsOriginAlignment =>
        GameObject.OriginAlignment.Bottom | GameObject.OriginAlignment.Center;
    
    public LayerDescription[] Layers { get; } =
    [
    ];

    public ObjectDescription[] Objects { get; } =
    [
    ];

    public string[] TileSets { get; } =
    [
        "assets:/Game/Tilesets/Forest.png",
        "assets:/Game/Tilesets/Caves.png"
    ];
    
    public ImageDescription[] Images { get; } =
    [
        new ("Big Tree", "assets:/Game/Images/BigTree")
    ];

    public MaterialInfo[] Materials { get; } =
    [
        
    ];
    
    public int PreviewZoom => 2;
    public decimal TilesetPanelZoom => 2;
}