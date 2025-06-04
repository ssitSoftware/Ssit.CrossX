using System;
using System.Numerics;
using Nokemono.Core.Game;
using Nokemono.Core.Game.Objects;
using Nokemono.Core.Game.Objects.Devices;
using Nokemono.Core.Game.Objects.Npc;
using Nokemono.Core.Game.Objects.Pushables;
using Ssit.CrossX;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.IO;

namespace Nokemono.Core;

public class GameTemplate: IGameTemplate
{
    public Vector2 Gravity => new(0, GamePhysics.GravityAcceleration);
    
    public string Name => "Retro Gunslinger";
    public Guid Guid { get; } = Guid.Parse("da59cd55-3cc6-4674-ab9e-3938d239c9c3");

    public IFilesProvider AssetsProvider { get; } = new AggregatedFilesProvider()
        .AddProvider("assets:",
            new EmbeddedFilesProvider(typeof(GameTemplate).Assembly, "Nokemono.Core.Assets"))
        .AddProvider("bundle:", new BundleFilesProvider());
    
    public int TileSize => 16;
    public Size TargetSize => new(480, 270);
    
    public RgbaColor DefaultBackground => new(0x000000);
    public RgbaColor EmptyColor => new(96, 64, 96);

    public int TrimToPixels => 32;
    
    public GameObject.OriginAlignment ObjectsOriginAlignment =>
        GameObject.OriginAlignment.Bottom | GameObject.OriginAlignment.Center;
    
    public LayerDescription[] Layers { get; } =
    [
        new ("B", "- Background", new Size(128,96), 0.5f, 0.5f, "Background", 100, RgbaColor.White, LayerAlign.Bottom),
        
        new ("CB2", "Close Background 2", new Size(512,96),  1, 1, "Tileset", 10, RgbaColor.White, LayerAlign.Bottom),
        new ("CB", "Close Background", new Size(512,96),  1, 1, "Tileset", 10, RgbaColor.White, LayerAlign.Bottom),
        new (LayerDescription.MainLayerId, "Main", new Size(512,96), 1, 1, "Tileset", 0, RgbaColor.White, LayerAlign.Left),
        new ("CF", "Close Foreground", new Size(512,96), 1, 1, "Tileset", -10, RgbaColor.White, LayerAlign.Bottom),
        new ("CF2", "Close Foreground 2", new Size(512,96), 1, 1, "Tileset", -10, RgbaColor.White, LayerAlign.Bottom),
    ];

    public ObjectDescription[] Objects { get; } =
    [
        new("Player", typeof(Player), "assets:/Game/Objects/SwordMaster", "Idle", typeof(Player.Parameters)),
        new("NPC/Merchant", typeof(Merchant), "assets:/Game/Objects/Merchant", "Idle"),
        new("Logic/Target", typeof(Target), "assets:/Editor/Target", "Default", typeof(Target.Parameters)),
        new("Logic/Story Trigger", typeof(StoryTrigger), "assets:/Editor/StoryTrigger", "Default", typeof(StoryTrigger.Parameters)),
        new("Devices/Elevator", typeof(ElevatorImpl), "assets:/Game/Objects/Elevator", "Off", typeof(Elevator.Parameters)),
        // new("Devices/Switch", typeof(SwitchImpl), "assets:/Game/Objects/Switch", "Off", typeof(Switch.Parameters)),
        new("Devices/Virtual Switch", typeof(VirtualSwitch), "assets:/Editor/LogicalSwitch", "Toggle", typeof(VirtualSwitch.Parameters)),
        new("Devices/Switch Aggregator", typeof(SwitchAggregator), "assets:/Editor/LogicalSwitch", "Logical", typeof(SwitchAggregator.Parameters)),
        // new("Devices/Metal Door", typeof(MechanicalDoorImpl), "assets:/Game/Objects/Door", "Closed", typeof(MechanicalDoor.Parameters)),
        // new("Devices/Detector", typeof(DetectorImpl), "assets:/Game/Objects/Detector", "Off"),
        // new("Elements/Power Plant", typeof(PowerPlant), "assets:/Game/Objects/PowerPlant", "On"),
        new("Elements/Crate", typeof(CrateImpl), "assets:/Game/Objects/Crate", "Preview", typeof(CrateImpl.Parameters)),
        // new("Elements/Tire", typeof(TireImpl), "assets:/Game/Objects/Tire", "Preview")
    ];

    public string[] TileSets { get; } =
    [
        "assets:/Game/Tilesets/Tileset.png"
    ];

    public ImageDescription[] Images { get; } =
    [
        new ("Trees/Big Tree 1", "assets:/Game/Images/BigTree1"),
        new ("Trees/Big Tree 2", "assets:/Game/Images/BigTree2"),
        new ("Statues/Woman Statue", "assets:/Game/Images/Statue1"),
        new ("Statues/Angel Statue", "assets:/Game/Images/Statue2"),
        new ("Statues/Fallen Statue", "assets:/Game/Images/Statue3"),
        // new ("Foreground/Big Tree 01", "assets:/Game/Images/BigTree01"),
        // new ("Foreground/Big Tree 02", "assets:/Game/Images/BigTree02"),
        // new ("Foreground/Big Tree 03", "assets:/Game/Images/BigTree03"),
        // new ("Monuments/Tomb", "assets:/Game/Images/Tomb"),
        // new ("Monuments/Chapel", "assets:/Game/Images/Chapel"),
        // new ("Monuments/Blue Throne", "assets:/Game/Images/Throne", "Blue"),
        // new ("Monuments/Green Throne", "assets:/Game/Images/Throne", "Green"),
        // new ("Background/Small Throne", "assets:/Game/Images/ThroneSmall", "Green"),
        // new ("Light/Torch", "assets:/Game/Images/Torch"),
        // new ("Background/Moon", "assets:/Game/Images/Moon"),
    ];

    public MaterialInfo[] Materials { get; } =
    [
        new("Default", null, RgbaColor.LightBlue, RgbaColor.Yellow),
        new("Wood", "WOOD", RgbaColor.SaddleBrown, RgbaColor.Orange),
        new("Mud", "MUD", RgbaColor.DarkKhaki, RgbaColor.Coral),
        new("Water", "WAT|ER", RgbaColor.LightBlue, RgbaColor.LightBlue),
        new("Platform", "PLAT|FORM", RgbaColor.Gray, RgbaColor.BlueViolet),
        new("Wood Platform", "PLAT|WOOD", RgbaColor.SaddleBrown, RgbaColor.LightGoldenrodYellow),
        new("Metal Platform", "PLAT|METL", RgbaColor.DarkGray, RgbaColor.LightGoldenrodYellow),
        new("Hurt", "HURT", RgbaColor.Red, RgbaColor.OrangeRed),
        new("Ladder", "LAD|DER", RgbaColor.SandyBrown, RgbaColor.SandyBrown)
    ];
    
    public int PreviewZoom => 2;
    public decimal TilesetPanelZoom => 2;
}