using Ssit.CrossX.IO;

namespace Ssit.CrossX.Games;

public interface IGameTemplate
{
    string Name { get; }
    int TileSize { get; }
    IFilesProvider FilesProvider { get; }
    (string name, string path) [] Tilesets { get; }
    (string name, string path) [] Images { get; }
    GameObject.OriginAlignment ObjectsOriginAlignment { get; }
    RgbaColor TilesBackgroundColor { get; }
    RgbaColor PreviewBackgroundColor { get; }
}