using System;
using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Games.Rendering.Map;

public class LayerDisplayElement: IDisposable
{
    public IReadOnlyList<(RectangleF bounds, TilesDisplaySegment[] segments)> Tiles => _tiles;
    private readonly List<(RectangleF bounds, TilesDisplaySegment[] segments)> _tiles;
    
    public Vector2 Speed { get; }
    public RgbaColor FogColor { get; }
    public Size SourceSize { get; }
    public bool IsMain { get; }
    
    internal LayerDisplayElement(List<(RectangleF bounds, TilesDisplaySegment[] segments)> tiles, Vector2 speed, RgbaColor fogColor, Size sourceSize, bool mainLayer)
    {
        _tiles = tiles;
        Speed = speed;
        FogColor = fogColor;
        SourceSize = sourceSize;
        IsMain = mainLayer;
    }

    public void Dispose()
    {
        foreach (var tile in _tiles)
        {
            foreach (var segment in tile.segments)
            {
                segment.Dispose();
            }
        }
        _tiles.Clear();
    }

    public void Update(float dt)
    {
    }
}