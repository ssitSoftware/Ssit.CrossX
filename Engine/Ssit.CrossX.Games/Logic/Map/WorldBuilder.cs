using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.IO;

namespace Ssit.CrossX.Games.Logic.Map;

public class WorldBuilder
{
    private MapFile _mapFile;
    private IFilesProvider _filesProvider;
    private IGameTemplate _gameTemplate;

    public WorldBuilder WithGameTemplate(IGameTemplate gameTemplate)
    {
        _gameTemplate = gameTemplate;
        return this;
    }
    
    public WorldBuilder WithFilesProvider(IFilesProvider filesProvider)
    {
        _filesProvider = filesProvider;
        return this;
    }
    
    public WorldBuilder WithMap(MapFile file)
    {
        _mapFile = file;
        return this;
    }

    public World Build()
    {
        var world = new World(_gameTemplate.Gravity);
        var tilesets = _mapFile.Tilesets.Select(o =>
        {
            using var stream = _filesProvider.Open(PathHelper.GetPathWithExtension(o, "tiles"));
            return TilesetMeta.FromStream(stream);
        }).ToArray();

        var layer = _mapFile.MainLayer;
        var collisions = new Dictionary<string, List<Vector2[]>>();

        for (var ix = 0; ix < layer.Width; ++ix)
        {
            for (var iy = 0; iy < layer.Height; ++iy)
            {
                var tile = layer.Tiles[ix, iy];
                if (tile.IsEmpty)
                {
                    continue;
                }

                var col = tilesets[tile.TileSet].GetCollisionPolygon(tile.X, tile.Y);
                var material = tilesets[tile.TileSet].GetMaterial(tile.X, tile.Y);

                if (col == null || material == null)
                {
                    continue;
                }

                var offset = new Vector2(ix, iy);
                col = col.Select( o=> o + offset).ToArray();

                if (!collisions.TryGetValue(material, out var list))
                {
                    list = new List<Vector2[]>();
                    collisions.Add(material, list);
                }

                list.Add(col);
            }
        }

        var materials = _gameTemplate.Materials.ToList();
        
        foreach (var pair in collisions)
        {
            var lines = PolygonMerge.Merge(pair.Value, 1000, 1000);
            var staticBody = new Body(world, Vector2.Zero);
            
            staticBody.MaterialIndex = materials.FindIndex(o=>o.Name == pair.Key);
            
            foreach (var col in lines)
            {
                switch (col.Item1.Length)
                {
                    case 0:
                    case 1:
                        throw new InvalidDataException();
                        
                    case 2:
                        staticBody.CreateFixture(new EdgeShape(col.Item1[0], col.Item1[1]));
                        break;
                    
                    default:
                        staticBody.CreateFixture(new ChainShape(new Vertices(col.Item1), col.Item2));
                        break;
                }
            }
        }
        
        return world;
    }
}