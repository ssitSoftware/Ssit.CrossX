using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Games.Rendering.Map;

public class LayerDisplayElementBuilder
{
    private MapLayer _layer;
    private MapFile _file;
    
    private IIoCContainer _container;
    private IContentManager _contentManager;
    
    private int _tileSize;
    private Size _targetSize;
    
    public LayerDisplayElementBuilder WithServices(IIoCContainer container, IContentManager contentManager)
    {
        _container = container;
        _contentManager = contentManager;
        return this;
    }
    
    public LayerDisplayElementBuilder WithMap(MapFile mapFile)
    {
        _file = mapFile;
        return this;
    }
    
    public LayerDisplayElementBuilder WithLayer(MapLayer layer)
    {
        _layer = layer;
        return this;
    }
    
    public LayerDisplayElementBuilder WithTemplate(IGameTemplate template)
    {
        _tileSize = template.TileSize;
        _targetSize = template.TargetSize;
        
        return this;
    }

    public LayerDisplayElement Build()
    {
        var list = new List<(RectangleF bounds, TilesDisplaySegment[] segments)>();
        var objects = new List<MapDisplayObject>();
        
        var segmentWidth = (int)MathF.Ceiling((float)_targetSize.Width / _tileSize / 2);
        var segmentHeight = (int)MathF.Ceiling((float)_targetSize.Height / _tileSize / 2);

        for (var oy = 0; oy < _layer.Height; oy += segmentHeight)
        {
            var height = Math.Min(segmentHeight, _layer.Height - oy);
            for (var ox = 0; ox < _layer.Width; ox += segmentWidth)
            {
                var width = Math.Min(segmentWidth, _layer.Width - ox);
                var bounds = new Rectangle(ox, oy, width, height);
                var segments = GenerateSegment(bounds);
                list.Add((bounds, segments));
            }
        }

        foreach (var obj in _layer.Objects)
        {
            if (!obj.HasLogic)
            {
                var dispObj = _container.IoCConstruct<MapDisplayObject>(obj);
                dispObj.Depth = _layer.Depth;
                dispObj.TintColor = _layer.TintColor.AsPremultiplied();
                objects.Add(dispObj);
            }
        }
        
        return new LayerDisplayElement(list, objects.OrderBy( o=>o.Name).ToList(), new Vector2(_layer.HorizontalSpeed, _layer.VerticalSpeed), _layer.FogColor.AsPremultiplied(), _layer.Size, _layer == _file.MainLayer);
    }

    private TilesDisplaySegment[] GenerateSegment(Rectangle rect)
    {
        var builder = new TilesDisplaySegmentBuilder()
            .WithServices(_container, _contentManager)
            .WithBounds(rect)
            .WithMap(_file)
            .WithLayer(_layer)
            .WithTileSize(_tileSize);

        return builder.Build();
    }
}