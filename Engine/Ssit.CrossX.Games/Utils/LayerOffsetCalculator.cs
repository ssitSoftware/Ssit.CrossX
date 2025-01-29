using System;
using System.Numerics;
using Ssit.CrossX.Games.Map;

namespace Ssit.CrossX.Games.Utils;

public static class LayerOffsetCalculator
{
    public static Vector2 CalculateLayerOffset(Vector2 layerSpeed, Size layerSize, Size mainLayerSize, Vector2 globalOffset)
    {
        float offX = globalOffset.X * layerSpeed.X;
        
        float vertSpeed = layerSpeed.Y;
        var offY = globalOffset.Y * vertSpeed - mainLayerSize.Height * vertSpeed + layerSize.Height;

        return new(offX, offY);
    }
    
    public static Vector2 CalculateLayerOffset(MapLayer layer, MapLayer mainLayer, Vector2 globalOffset)
    {
        float offX = globalOffset.X * (float)layer.HorizontalSpeed;
        
        float vertSpeed = MathF.Max(0.0001f, (float) layer.VerticalSpeed);
        var offY = globalOffset.Y * vertSpeed - mainLayer.Height * vertSpeed + layer.Height;

        return new(offX, offY);
    }
    
    public static Vector2 CalculateGlobalOffset(MapLayer layer, MapLayer mainLayer, Vector2 localOffset)
    {
        float horzSpeed = MathF.Max(0.0001f, (float) layer.HorizontalSpeed);
        float vertSpeed = MathF.Max(0.0001f, (float) layer.VerticalSpeed);
        
        
        float offX = localOffset.X / horzSpeed;
        float offY = mainLayer.Height - (layer.Height - localOffset.Y) / vertSpeed;

        return new(offX, offY);
    }
}