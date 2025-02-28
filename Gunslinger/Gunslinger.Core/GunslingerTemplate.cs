using System;
using System.Numerics;
using Gunslinger.Core.Game;
using Gunslinger.Core.Game.Objects;
using Gunslinger.Core.Game.Objects.Devices;
using Gunslinger.Core.Game.Objects.Pushables;
using Ssit.CrossX;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.IO;

namespace Gunslinger.Core;

public class GunslingerTemplate: IGameTemplate
{
    public Vector2 Gravity => new(0, GamePhysics.GravityAcceleration);
    
    public string Name => "Gunslinger";
    public Guid Guid { get; } = Guid.Parse("b7d05cc4-a3f3-461f-8cb2-07ec58b6120a");

    public IFilesProvider AssetsProvider { get; } = new AggregatedFilesProvider()
        .AddProvider("assets:",
            new EmbeddedFilesProvider(typeof(GunslingerTemplate).Assembly, "Gunslinger.Core.Assets"))
        .AddProvider("bundle:", new BundleFilesProvider());
    
    public int TileSize => 16;
    public Size TargetSize => new(512, 288);
    
    public RgbaColor DefaultBackground => new(0xff404040);
    public RgbaColor EmptyColor => new(128, 128, 128);
    
    public GameObject.OriginAlignment ObjectsOriginAlignment =>
        GameObject.OriginAlignment.Bottom | GameObject.OriginAlignment.Center;
    
    public LayerDescription[] Layers { get; } =
    [
        new ("FFB", "--- Furthest Background", new Size(30,23), 0, 0, "Sky", 1000, RgbaColor.White, LayerAlign.Top, new RgbaColor(224,232,224,56)),
        
        new ("FB", "-- Far Background", new Size(64,48), 0.25f, 0.25f, "Background", 250, new RgbaColor(224,232,224), LayerAlign.Bottom),
        new ("FBO", "-- Far Background Overlay", new Size(64,48), 0.25f, 0.25f, "Background", 250, new RgbaColor(224,232,224), LayerAlign.Bottom, new RgbaColor(224,232,224,56)),
        
        new ("B", "- Background", new Size(128,96), 0.5f, 0.5f, "Background", 100, new RgbaColor(224,232,224), LayerAlign.Bottom),
        new ("BO", "- Background Overlay", new Size(128,96), 0.5f, 0.5f, "Background", 100, new RgbaColor(224,232,224), LayerAlign.Bottom, new RgbaColor(224,232,224,56)),
        
        new ("CB", "Close Background", new Size(512,96),  1, 1, "Main", 10, RgbaColor.White, LayerAlign.Bottom),
        new (LayerDescription.MainLayerId, "Main", new Size(512,96), 1, 1, "Main", 0, RgbaColor.White, LayerAlign.Left),
        new ("CF", "Close Foreground", new Size(512,96), 1, 1, "Main", -10, RgbaColor.White, LayerAlign.Bottom),
        new ("CFO", "Close Foreground Overlay", new Size(512,96), 1, 1, "Main", -10, RgbaColor.White, LayerAlign.Bottom),

        new ("FG", "- Foreground", new Size(512,96), 2f, 2f, "Foreground", -100, RgbaColor.Black, LayerAlign.Bottom)
    ];

    public ObjectDescription[] Objects { get; } =
    [
        new("Player", typeof(Player), "assets:/Game/Objects/SwordMaster", "Idle", typeof(Player.Parameters)),
        new("Logic/Target", typeof(Target), "assets:/Editor/Target", "Default", typeof(Target.Parameters)),
        new("Devices/Elevator", typeof(ElevatorImpl), "assets:/Game/Objects/Elevator", "Off", typeof(Elevator.Parameters)),
        new("Devices/Switch", typeof(SwitchImpl), "assets:/Game/Objects/Switch", "Off", typeof(Switch.Parameters)),
        new("Devices/Virtual Switch", typeof(VirtualSwitch), "assets:/Editor/LogicalSwitch", "Toggle", typeof(VirtualSwitch.Parameters)),
        new("Devices/Switch Aggregator", typeof(SwitchAggregator), "assets:/Editor/LogicalSwitch", "Logical", typeof(SwitchAggregator.Parameters)),
        new("Devices/Metal Door", typeof(MechanicalDoorImpl), "assets:/Game/Objects/Door", "Closed", typeof(MechanicalDoor.Parameters)),
        new("Devices/Detector", typeof(DetectorImpl), "assets:/Game/Objects/Detector", "Off"),
        new("Elements/Power Plant", typeof(PowerPlant), "assets:/Game/Objects/PowerPlant", "On"),
        new("Elements/Crate", typeof(CrateImpl), "assets:/Game/Objects/Crate", "Preview", typeof(CrateImpl.Parameters)),
        new("Elements/Tire", typeof(TireImpl), "assets:/Game/Objects/Tire", "Preview")
    ];

    public string[] TileSets { get; } =
    [
        "assets:/Game/Tilesets/Wild.png",
        "assets:/Game/Tilesets/City.png",
        "assets:/Game/Tilesets/Victorian.png",
        "assets:/Game/Tilesets/Background.png",
        "assets:/Game/Tilesets/Beneath.png",
        "assets:/Game/Tilesets/Cables.png",
    ];

    public ImageDescription[] Images { get; } =
    [
        new ("Trees/Tree 01", "assets:/Game/Images/Tree01"),
        new ("Trees/Tree 02", "assets:/Game/Images/Tree02"),
        new ("Trees/Tree 03", "assets:/Game/Images/Tree03"),
        new ("Foreground/Big Tree 01", "assets:/Game/Images/BigTree01"),
        new ("Foreground/Big Tree 02", "assets:/Game/Images/BigTree02"),
        new ("Foreground/Big Tree 03", "assets:/Game/Images/BigTree03"),
        new ("Monuments/Tomb", "assets:/Game/Images/Tomb"),
        new ("Monuments/Chapel", "assets:/Game/Images/Chapel"),
        new ("Monuments/Blue Throne", "assets:/Game/Images/Throne", "Blue"),
        new ("Monuments/Green Throne", "assets:/Game/Images/Throne", "Green"),
        new ("Background/Small Throne", "assets:/Game/Images/ThroneSmall", "Green"),
        new ("Light/Torch", "assets:/Game/Images/Torch"),
        new ("Background/Moon", "assets:/Game/Images/Moon"),
    ];

    public MaterialInfo[] Materials { get; } =
    [
        new("Default", null, RgbaColor.LightBlue, RgbaColor.Yellow),
        new("Grass", "GR|ASS", RgbaColor.Green, RgbaColor.GreenYellow),
        new("Wood", "WO|OD", RgbaColor.SaddleBrown, RgbaColor.Orange),
        new("Mud", "MUD", RgbaColor.DarkKhaki, RgbaColor.Coral),
        new("Water", "WAT|ER", RgbaColor.LightBlue, RgbaColor.LightBlue),
        new("Ice", "ICE", RgbaColor.AliceBlue, RgbaColor.AliceBlue),
        new("Wood Platform", "PLA|WDN", RgbaColor.SaddleBrown, RgbaColor.LightGoldenrodYellow),
        new("Metal Platform", "PLA|MET", RgbaColor.DarkGray, RgbaColor.LightGoldenrodYellow)
    ];
    
    public int PreviewZoom => 2;
    public decimal TilesetPanelZoom => 2;
}