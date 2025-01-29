using System;
using Ssit.CrossX;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Map;
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
        new ("--- Furthest Background", new Size(30,23), 0, 0, "Sky", 1000, RgbaColor.White, LayerAlign.Top),
        new ("-- Far Background", new Size(64,48), 0.25f, 0.25f, "Background", 250, RgbaColor.White, LayerAlign.Bottom),
        new ("- Background", new Size(128,96), 0.5f, 0.5f, "Background", 100, RgbaColor.White, LayerAlign.Bottom),

        new ("Close Background", new Size(512,96), 1, 1, "Main", 10, RgbaColor.White, LayerAlign.Bottom),
        new ("Main", new Size(512,96), 1, 1, "Main", 0, RgbaColor.White, LayerAlign.Left),
        new ("Close Foreground", new Size(512,96), 1, 1, "Main", -10, RgbaColor.White, LayerAlign.Bottom),

        new ("- Foreground", new Size(512,96), 2f, 2f, "Foreground", -100, RgbaColor.Black, LayerAlign.Bottom)
    ];

    public ObjectDescription[] Objects { get; } =
    [
    ];

    public string[] TileSets { get; } =
    [
        "assets:/Game/Tilesets/Wild.png",
        "assets:/Game/Tilesets/City.png",
        "assets:/Game/Tilesets/Victorian.png"
    ];
    
    public ImageDescription[] Images { get; } =
    [
        new ("Tree 01", "assets:/Game/Images/Tree01"),
        new ("Tree 02", "assets:/Game/Images/Tree02"),
        new ("Tree 03", "assets:/Game/Images/Tree03"),
        new ("assets:/Game/Images/Tomb"),
        new ("assets:/Game/Images/Chapel")
    ];

    public MaterialInfo[] Materials { get; } = {
        new("Default", null, RgbaColor.LightBlue),
        new("Grass", "GR|ASS", RgbaColor.Green),
        new("Wood", "WO|OD", RgbaColor.SaddleBrown),
        new("Mud", "MUD", RgbaColor.DarkKhaki),
        new("Ice", "ICE", RgbaColor.AliceBlue),
        new("Wood Platform", "PLA|WDN", RgbaColor.SaddleBrown),
        new("Metal Platform", "PLA|MET", RgbaColor.DarkGray)
    };
    
    public int PreviewZoom => 2;
    public decimal TilesetPanelZoom => 2;
}