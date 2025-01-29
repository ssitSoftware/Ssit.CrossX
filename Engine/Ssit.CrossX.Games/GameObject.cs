using System;
using System.IO;
using System.Numerics;
using SkiaSharp;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.Graphics.Sprites.Json;
using Ssit.CrossX.IO;

namespace Ssit.CrossX.Games;

public class GameObject: IDisposable
{
    [Flags]
    public enum OriginAlignment
    {
        Default = 0,
        Center = 1,
        VCenter = 2,
        Right = 4,
        Bottom = 8
    }
    
    private readonly IContentManager _contentManager;

    public readonly ObjectDescription Description;

    public readonly bool HasSprite;
    public readonly bool HasTexture;

    public readonly string ResourcePath;

    public static (ObjectDescription, string, bool, bool) Parse(string path, IFilesProvider filesProvider, OriginAlignment originAlignment = OriginAlignment.Center | OriginAlignment.VCenter)
    {
        var hasSprite = filesProvider.FileExists(path + ".json");
        var hasTexture = false;
        
        var resourcePath = "";
        
        if (hasSprite)
        {
            resourcePath = path + ".json";
        }
        else
        {
            hasTexture = filesProvider.FileExists(path + ".png");
            if (hasTexture)
            {
                resourcePath = path + ".png";
            }
        }
        
        Size sourceSize = Size.Zero;
        
        if (hasSprite)
        {
            var sprite = (Sprite)JsonSpriteLoader.Load(resourcePath, filesProvider);
            sourceSize = sprite.SourceSize;
        }

        if (hasTexture)
        {
            using var stream = filesProvider.Open(resourcePath);
            using var bmp = SKBitmap.Decode(stream);
            sourceSize = new Size(bmp.Width, bmp.Height);
        }
        
        ObjectDescription desc;
        try
        {
            using var stream = filesProvider.Open(path + ".desc.json");
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            desc = new ObjectDescription(json, sourceSize);
        }
        catch
        {
            var origin = Vector2.Zero;

            if (originAlignment.HasFlag(OriginAlignment.Center))
            {
                origin.X = sourceSize.Width / 2f;
            }
            
            if (originAlignment.HasFlag(OriginAlignment.Right))
            {
                origin.X = sourceSize.Width;
            }
            
            if (originAlignment.HasFlag(OriginAlignment.VCenter))
            {
                origin.Y = sourceSize.Height / 2f;
            }
            
            if (originAlignment.HasFlag(OriginAlignment.Bottom))
            {
                origin.Y = sourceSize.Height;
            }
            
            desc = new ObjectDescription(origin, sourceSize);
        }
        
        return (desc, resourcePath, hasSprite, hasTexture);
    }
    
    public static GameObject FromPath(string path, IFilesProvider filesProvider) => new(Parse(path, filesProvider));
    
    private GameObject( (ObjectDescription description, string resourcePath, bool hasSprite, bool hasTexture) data)
    {
        Description = data.description;
        ResourcePath = data.resourcePath;
        HasSprite = data.hasSprite;
        HasTexture = data.hasTexture;
    }
    
    internal GameObject(string path, IContentManager contentManager): this(Parse(path, contentManager.FilesProvider))
    {
        _contentManager = contentManager;
    }
    
    public SpriteInstance CreateSpriteInstance()
    {
        if ( _contentManager is null) throw new InvalidOperationException("GameObject has no content manager");
        if (!HasSprite) throw new InvalidOperationException("GameObject has no sprite");
        return new SpriteInstance(ResourcePath, Description.Origin, _contentManager);
    }

    public ResourceHandle<ITexture> RequestTexture()
    {
        if ( _contentManager is null) throw new InvalidOperationException("GameObject has no content manager");
        return _contentManager.Get<ITexture>(ResourcePath);
    }
    
    void IDisposable.Dispose()
    {
    }
}