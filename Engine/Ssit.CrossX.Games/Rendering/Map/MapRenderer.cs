using System;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Utils;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games.Rendering.Map;

public static class MapRenderer
{
    public static void Render(IRenderer2 renderer, MapDisplayElement map, World world, Vector2 cameraLookAt, Size targetSize, int tileSize)
    {
        var mainLayer = map.Layers.First(o => o.IsMain);
        var screenSizeNormalized = targetSize.ToVector() / tileSize;
        var globalOffset = cameraLookAt - screenSizeNormalized / 2;
        
        globalOffset.X = MathF.Min(MathF.Max(0, globalOffset.X), mainLayer.SourceSize.Width - screenSizeNormalized.X);
        globalOffset.Y = MathF.Min(MathF.Max(screenSizeNormalized.Y, globalOffset.Y), mainLayer.SourceSize.Height);
        
        foreach (var layer in map.Layers)
        {
            var offset = LayerOffsetCalculator.CalculateLayerOffset(layer, mainLayer, globalOffset);
            
            offset = -offset * tileSize + targetSize.ToVector() / 2 + new Vector2(-targetSize.Width / 2f, targetSize.Height / 2f);
            var visibleBounds = new RectangleF(-offset.X / tileSize, -offset.Y / tileSize, (float)targetSize.Width / tileSize, (float)targetSize.Height / tileSize);
            
            renderer.StateManager.SaveState();
            renderer.StateManager.Translate(offset);
            
            RenderLayer(renderer, layer, visibleBounds);

            if (layer.IsMain)
            {
                RenderGameObjects(renderer, world, visibleBounds);
            }
            
            renderer.StateManager.RestoreState();
            
            if (layer.FogColor.A > 0)
            {
                renderer.GeometryRenderer.FillRectangle(new RectangleF(Vector2.Zero, targetSize), renderer.StateProvider.UseGlowTextures ? RgbaColor.Black * layer.FogColor.Af: layer.FogColor);
            }
        }
    }

    private static void RenderLayer(IRenderer2 renderer, LayerDisplayElement layer, RectangleF bounds)
    {
        foreach (var tiles in layer.Tiles)
        {
            if (tiles.bounds.IsIntersecting(bounds))
            {
                foreach (var segment in tiles.segments)
                {
                    renderer.QuadsRenderer.Draw(segment.Texture.Resource, segment.Quads, segment.TintColor);
                }
            }
        }

        foreach (var obj in layer.DisplayObjects)
        {
            DrawObject(renderer, bounds, obj);
        }
    }

    private static void DrawObject(IRenderer2 renderer, RectangleF bounds, MapDisplayObject obj)
    {
        // TODO: Filter invisible elements
        if (obj.Texture is not null)
        {
            renderer.SpriteRenderer.Draw(obj.Texture, obj.Position, null, obj.Origin, color: obj.TintColor,  imageTransform: obj.IsFlipped ? ImageTransform.FlipHorizontal : ImageTransform.None);
        }

        if (obj.SpriteInstance is not null)
        {
            renderer.SpriteRenderer.Draw(obj.SpriteInstance, obj.Position, color: obj.TintColor, transform: obj.IsFlipped ? ImageTransform.FlipHorizontal : ImageTransform.None);
        }
    }

    private static void RenderGameObjects(IRenderer2 renderer,
        World world, RectangleF bounds)
    {
        if (world is null)
            return;
        
        foreach (var body in world.BodyList)
        {
            if (body.UserData is IGameObjectRenderer2 gor && gor.Bounds.IsIntersecting(bounds))
            {
                gor.Render(renderer);
            }
        }
    }
}