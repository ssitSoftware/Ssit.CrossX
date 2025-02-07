using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Games.Rendering.Map;

public class TilesDisplaySegmentBuilder
{
    private readonly Dictionary<int, List<VertexPositionColorTexture>> _verticesMap = new();
    
    private string[] _tileSets;
    private int _startX;
    private int _startY;
    
    private int _endX;
    private int _endY;

    private int _tileSize;
    
    private RgbaColor _tintColor;
    private Tile[,] _tiles;
    
    private IIoCContainer _container;
    private IContentManager _contentManager;

    private float _depth;

    public TilesDisplaySegmentBuilder WithServices(IIoCContainer container, IContentManager contentManager)
    {
        _container = container;
        _contentManager = contentManager;
        return this;
    }
    
    public TilesDisplaySegmentBuilder WithMap(MapFile mapFile)
    {
        _tileSets = mapFile.Tilesets;
        return this;
    }
    
    public TilesDisplaySegmentBuilder WithLayer(MapLayer layer)
    {
        _tintColor = layer.TintColor;
        _tiles = layer.Tiles;
        _depth = layer.Depth;
        
        return this;
    }

    public TilesDisplaySegmentBuilder WithTileSize(int tileSize)
    {
        _tileSize = tileSize;
        return this;
    }

    public TilesDisplaySegmentBuilder WithBounds(Rectangle bounds)
    {
        _startX = bounds.X;
        _startY = bounds.Y;
        _endX = bounds.Right;
        _endY = bounds.Bottom;
        
        return this;
    }

    public IEnumerable<TilesDisplaySegment> Build()
    {
        var textures = 
            _tileSets.Select(tileset => _contentManager.Get<ITexture>(tileset)).ToArray();

        try
        {
            _verticesMap.Clear();

            for (var xx = _startX; xx < _endX; xx++)
            {
                for (var yy = _startY; yy < _endY; yy++)
                {
                    var tile = _tiles[xx, yy];
                    if (tile.IsEmpty)
                    {
                        continue;
                    }

                    var tl = new Vector2(xx * _tileSize, yy * _tileSize);
                    var tr = tl + new Vector2(_tileSize, 0);
                    var br = tl + new Vector2(_tileSize, _tileSize);
                    var bl = tl + new Vector2(0, _tileSize);

                    var xox = (float)_tileSize / textures[tile.TileSet].Resource.Size.Width;
                    var xoy = (float)_tileSize / textures[tile.TileSet].Resource.Size.Height;

                    var xtl = new Vector2((float)tile.X * _tileSize / textures[tile.TileSet].Resource.Size.Width,
                        (float)tile.Y * _tileSize / textures[tile.TileSet].Resource.Size.Height);

                    var xtr = tl + new Vector2(xox, 0);
                    var xbr = tl + new Vector2(xox, xoy);
                    var xbl = tl + new Vector2(0, xoy);

                    if (!_verticesMap.TryGetValue(tile.TileSet, out List<VertexPositionColorTexture> vertices))
                    {
                        vertices = new List<VertexPositionColorTexture>();
                        _verticesMap.Add(tile.TileSet, vertices);
                    }

                    vertices.Add(new VertexPositionColorTexture(new Vector3(tl, _depth), _tintColor, xtl));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(bl, _depth), _tintColor, xbl));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(tr, _depth), _tintColor, xtr));

                    vertices.Add(new VertexPositionColorTexture(new Vector3(tr, _depth), _tintColor, xtr));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(bl, _depth), _tintColor, xbl));
                    vertices.Add(new VertexPositionColorTexture(new Vector3(br, _depth), _tintColor, xbr));
                }
            }

            foreach (var (key, vertices) in _verticesMap)
            {
                var texture = _tileSets[key];

                yield return _container.IoCConstruct<TilesDisplaySegment>(new TilesDisplaySegment.Parameters
                {
                    Vertices = vertices.ToArray(),
                    TexturePath = texture
                });
            }
        }
        finally
        {
            _verticesMap.Clear();
            foreach (var tex in textures)
            {
                tex.Dispose();
            }
        }
    }
}