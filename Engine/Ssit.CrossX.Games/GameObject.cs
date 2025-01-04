using System;
using System.IO;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace Ssit.CrossX.Games;

public class GameObject: IDisposable
{
    private readonly IContentManager _contentManager;

    public ObjectDescription Description { get; }
    
    public bool HasSprite { get; }
    public bool HasTexture { get; }

    private readonly string _resourcePath;
    
    internal GameObject(string path, IContentManager contentManager)
    {
        var filesProvider = contentManager.FilesProvider;
        
        using var stream = filesProvider.Open(path + ".desc.json");
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        
        Description = new ObjectDescription(json);

        HasSprite = filesProvider.FileExists(path + ".json");

        if (HasSprite)
        {
            _resourcePath = path + ".json";
        }
        else
        {
            HasTexture = filesProvider.FileExists(path + ".png");
            if (HasTexture)
            {
                _resourcePath = path + ".png";
            }
        }
        
        _contentManager = contentManager;
    }
    
    public SpriteInstance CreateSpriteInstance()
    {
        if(!HasSprite) throw new InvalidOperationException("GameObject has no sprite");
        return new SpriteInstance(_resourcePath, Description.Origin, _contentManager);
    }

    public ResourceHandle<ITexture> RequestTexture()
    {
        return _contentManager.Get<ITexture>(_resourcePath);
    }
    
    void IDisposable.Dispose()
    {
    }
}