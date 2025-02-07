using System;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Utils;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.Games.Rendering.Map;

public static class MapRenderer
{
    public static void Render(IRenderer renderer, MapDisplayElement map, World world, Vector2 cameraLookAt, Size targetSize, int tileSize, RenderMode mode)
    {
        renderer.Flush();
        
        var mainLayer = map.Layers.First(o => o.IsMain);
        var screenSizeNormalized = targetSize.ToVector() / tileSize;
        var globalOffset = cameraLookAt - screenSizeNormalized / 2;
        
        globalOffset.X = MathF.Min(MathF.Max(0, globalOffset.X), mainLayer.SourceSize.Width - screenSizeNormalized.X);
        globalOffset.Y = MathF.Min(MathF.Max(0, globalOffset.Y), mainLayer.SourceSize.Height - screenSizeNormalized.Y);
        
        foreach (var layer in map.Layers)
        {
            var offset = LayerOffsetCalculator.CalculateLayerOffset(layer, mainLayer, globalOffset);
            var visibleBounds = new RectangleF(offset.X, offset.Y, (float)targetSize.Width / tileSize, (float)targetSize.Height / tileSize);
            
            renderer.StateManager.SaveState();
            renderer.StateManager.Transform(Matrix3x2.CreateTranslation(-offset * tileSize));
            
            RenderLayer(renderer, layer, visibleBounds, mode);

            if (layer.IsMain)
            {
                RenderGameObjects(renderer, world, visibleBounds, mode);
            }
            
            renderer.StateManager.RestoreState();
            
            if (layer.FogColor.A > 0)
            {
                renderer.FillRectangle(new RectangleF(Vector2.Zero, targetSize), mode == RenderMode.Normal ? layer.FogColor : RgbaColor.Black * layer.FogColor.Af);
            }
        }
    }

    private static void RenderLayer(IRenderer renderer, LayerDisplayElement layer,
        RectangleF bounds, RenderMode mode)
    {
        foreach (var tiles in layer.Tiles)
        {
            if (tiles.bounds.IsIntersecting(bounds))
            {
                foreach (var segment in tiles.segments)
                {
                    renderer.DrawPrimitives(segment.VertexBuffer, 0, segment.VertexBuffer.Length, segment.Texture.Resource, renderMode: mode);
                }
            }
        }
    }

    private static void RenderGameObjects(IRenderer renderer,
        World world, RectangleF bounds, RenderMode mode)
    {
        if (world is null)
            return;
        
        foreach (var body in world.BodyList)
        {
            if (body.UserData is IGameObjectRenderer2 gor && gor.Bounds.IsIntersecting(bounds))
            {
                gor.Render(renderer, mode);
            }
        }
    }
}