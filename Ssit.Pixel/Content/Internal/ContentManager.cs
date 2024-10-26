using System;
using System.Collections.Generic;
using System.IO;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.IO;
using Ssit.Pixel.IoC;

namespace Ssit.Pixel.Content.Internal;

internal class ContentManager: IContentManager
{
    private readonly IIoCContainer _ioCContainer;
    private readonly IFilesProvider _filesProvider;

    private class ResourceInstance
    {
        public IDisposable Object { get; }
        public readonly HashSet<Guid> Users = new();

        public ResourceInstance(IDisposable obj)
        {
            Object = obj;
        }
    }
    
    private readonly Dictionary<string, ResourceInstance> _resources = new();
    private readonly Dictionary<Type, LoadResourceDelegate> _resourceLoaders = new();

    public ContentManager(IIoCContainer ioCContainer, IFilesProvider filesProvider)
    {
        _ioCContainer = ioCContainer;
        _filesProvider = filesProvider;
        RegisterLoader<ITexture>(LoadTextureFunc);
    }

    public ResourceHandle<TResource> Load<TResource>(string path) where TResource : class, IDisposable
    {
        path = PathHelper.NormalizePath(path);
        
        var key = GetKey<TResource>(path);

        if (!_resources.TryGetValue(key, out var resource))
        {
            var obj = LoadResource<TResource>(path);

            resource = new ResourceInstance(obj);
            _resources.Add(key, resource);
        }

        var handle = new ResourceHandle<TResource>((TResource) resource.Object, key, g =>
        {
            resource.Users.Remove(g);
            if (resource.Users.Count == 0)
            {
                resource.Object.Dispose();
                _resources.Remove(key);
            }
        });

        resource.Users.Add(handle.Guid);
        return handle;
    }

    public void RegisterLoader<TResource>(LoadResourceDelegate loadFunc) where TResource : class, IDisposable 
        => _resourceLoaders.Add(typeof(TResource), loadFunc);

    private string GetKey<TResource>(string path) where TResource : class, IDisposable 
        => path + $" ({typeof(TResource).Name})";

    private TResource LoadResource<TResource>(string path) where TResource : class, IDisposable
    {
        var type = typeof(TResource);
        if (_resourceLoaders.TryGetValue(type, out var resourceLoader))
        {
            var resource = resourceLoader.Invoke(path);
            if (resource is not TResource resourceInstance)
            {
                throw new Exception($"Resource loading failed for type {type.Name} on path {path}");
            }

            return resourceInstance;
        }

        using var stream = _filesProvider.Open(path);
        return _ioCContainer.IoCConstruct<TResource>(stream);
    }
    
    private IDisposable LoadTextureFunc(string path)
    {
        var name = Path.Combine(Path.GetDirectoryName(path)!, Path.GetFileNameWithoutExtension(path));
        var ext = Path.GetExtension(path);

        var hasDiffuseImplicit = _filesProvider.FileExists(name + ext);
        var hasDiffuseExplicit = _filesProvider.FileExists(name + ".diffuse" + ext);
        var hasNormals = _filesProvider.FileExists(name + ".normal" + ext);

        return _ioCContainer.IoCConstruct<ITexture>(new LoadTextureParameters
        {
            DiffuseMapStream =  hasDiffuseImplicit ? _filesProvider.Open(name + ext) : hasDiffuseExplicit ? 
                _filesProvider.Open(name + ".diffuse" + ext) : null,
            
            NormalMapStream = hasNormals ? _filesProvider.Open(name + ".normal" + ext) : null
        });
    }
}