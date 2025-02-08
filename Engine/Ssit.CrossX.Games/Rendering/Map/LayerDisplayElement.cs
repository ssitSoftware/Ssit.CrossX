using System;
using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Games.Rendering.Map;

public class LayerDisplayElement: IDisposable
{
    public IReadOnlyList<(RectangleF bounds, TilesDisplaySegment[] segments)> Tiles => _tiles;
    private readonly List<(RectangleF bounds, TilesDisplaySegment[] segments)> _tiles;
    
    public IReadOnlyList<MapDisplayObject> DisplayObjects => _displayObjects;
    private readonly List<MapDisplayObject> _displayObjects;
    
    public Vector2 Speed { get; }
    public RgbaColor FogColor { get; }
    public Size SourceSize { get; }
    public bool IsMain { get; }
    
    internal LayerDisplayElement(List<(RectangleF bounds, TilesDisplaySegment[] segments)> tiles, 
        List<MapDisplayObject> displayObjects,
        Vector2 speed, RgbaColor fogColor, Size sourceSize, bool mainLayer)
    {
        _tiles = tiles;
        _displayObjects = displayObjects;
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
        
        foreach(var dispObj in _displayObjects)
        {
            dispObj.Dispose();
        }
        
        _displayObjects.Clear();
        _tiles.Clear();
    }

    public void Update(float dt)
    {
        foreach (var dispObj in _displayObjects)
        {
            dispObj.Update(dt);
        }
    }
}