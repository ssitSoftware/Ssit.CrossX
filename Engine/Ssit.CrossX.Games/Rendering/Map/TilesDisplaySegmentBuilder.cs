using System;
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
    private const int MergeSize = 32;
    
    private readonly Dictionary<int, List<VertexPositionColorTexture>> _verticesMap = new();
    private Tile?[,] _temp = new Tile?[MergeSize, MergeSize];
    
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

    public TilesDisplaySegment[] Build()
    {
        var list = new List<TilesDisplaySegment>();
        
        var textures = 
            _tileSets.Select(tileset => _contentManager.Get<ITexture>(tileset)).ToArray();
        
        try
        {
            _verticesMap.Clear();

            for (var xx = _startX; xx < _endX; xx++)
            {
                for (var yy = _startY; yy < _endY; yy++)
                {
                    var tile = GetTile(_tiles, xx, yy, out var w, out var h);
                    if (tile.IsEmpty)
                    {
                        continue;
                    }

                    var tl = new Vector2(xx * _tileSize, yy * _tileSize);
                    var tr = tl + new Vector2(_tileSize * w, 0);
                    var br = tl + new Vector2(_tileSize * w, _tileSize * h);
                    var bl = tl + new Vector2(0, _tileSize * h);

                    var xox = (float)_tileSize / textures[tile.TileSet].Resource.Size.Width;
                    var xoy = (float)_tileSize / textures[tile.TileSet].Resource.Size.Height;

                    var xtl = new Vector2(tile.X * xox, tile.Y * xoy);

                    var xtr = xtl + new Vector2(xox * w, 0);
                    var xbr = xtl + new Vector2(xox * w, xoy * h);
                    var xbl = xtl + new Vector2(0, xoy * h);

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

                list.Add(_container.IoCConstruct<TilesDisplaySegment>(new TilesDisplaySegment.Parameters
                {
                    Vertices = vertices.ToArray(),
                    TexturePath = texture
                }));
            }
            return list.ToArray();
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

    private Tile GetTile(Tile[,] tiles, int xx, int yy, out int width, out int height)
    {
        var maxX = Math.Min(xx + MergeSize, _endX);
        var maxY = Math.Min(yy + MergeSize, _endY);
        
        var countX = maxX - xx;
        var countY = maxY - yy;
        
        var tile = tiles[xx, yy];
        width = 0;
        height = 0;
        
        if (tile.IsEmpty) return tile;
        
        var tileset = tile.TileSet;
        
        for(var idx = xx; idx < maxX; ++idx)
        {
            for (var idy = yy; idy < maxY; ++idy)
            {
                var tile2 = tiles[idx, idy];
                if (tile2.TileSet != tileset)
                {
                    _temp[idx - xx, idy - yy] = null;
                    continue;
                }

                if (tile2.X != tile.X + idx - xx || tile2.Y != tile.Y + idy - yy)
                {
                    _temp[idx - xx, idy - yy] = null;
                    continue;
                }
                
                _temp[idx-xx, idy-yy] = tile2;
            }
        }

        width = 1;
        height = 1;

        for (var idx = 0; idx < 8; ++idx)
        {
            int left = 2;
            if (CheckFilled(width+1, height))
            {
                width++;
                left--;
            }
            
            if (CheckFilled(width, height+1))
            {
                height++;
                left--;
            }

            if (left == 2)
                break;
        }

        return tile;
    }

    private bool CheckFilled(int width, int height)
    {
        if (width > MergeSize || height > MergeSize)
            return false;
        
        for(var idx =0; idx < width; ++idx)
        {
            for (var idy = 0; idy < height; ++idy)
            {
                if (!_temp[idx, idy].HasValue)
                    return false;
            }
        }

        return true;
    }
}