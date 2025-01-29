using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games;
using Ssit.CrossX.IO;

namespace Gunslinger.Core;

public class GunslingerTemplate: IGameTemplate
{
    public string Name => "Gunslinger";
    
    public int TileSize => 16;
    
    public IFilesProvider FilesProvider { get; } = new AggregatedFilesProvider()
        .AddProvider("assets:", 
            new EmbeddedFilesProvider(typeof(GunslingerTemplate).Assembly, "Gunslinger.Core.Assets"));

    public (string name, string path) [] Tilesets { get; } =
    [
        ("Forest", "assets:/Game/Tilesets/Forest.png"),
        ("Caves", "assets:/Game/Tilesets/Caves.png")
    ];
    
    public (string name, string path) [] Images { get; } =
    [
        ("Big Tree", "assets:/Game/Images/BigTree")
    ];

    public GameObject.OriginAlignment ObjectsOriginAlignment =>
        GameObject.OriginAlignment.Bottom | GameObject.OriginAlignment.Center;

    public RgbaColor TilesBackgroundColor => new(192,96,192);
    public RgbaColor PreviewBackgroundColor => new(64, 64, 64);
}