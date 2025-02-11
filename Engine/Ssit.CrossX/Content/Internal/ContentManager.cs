using System;
using System.Collections.Generic;
using System.IO;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.Graphics.Sprites.Json;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Content.Internal;

internal class ContentManager: IContentManager
{
    private readonly IIoCContainer _ioCContainer;
    private readonly IFilesProvider _filesProvider;
    private readonly IActionScheduler _scheduler;

    public IFilesProvider FilesProvider => _filesProvider;
    
    private class ResourceInstance(IDisposable obj)
    {
        public IDisposable Object { get; } = obj;
        public readonly HashSet<Guid> Users = new();
    }
    
    private readonly Dictionary<string, ResourceInstance> _resources = new();
    private readonly Dictionary<Type, LoadResourceDelegate> _resourceLoaders = new();

    public ContentManager(IIoCContainer ioCContainer, IFilesProvider filesProvider, IActionScheduler scheduler)
    {
        _ioCContainer = ioCContainer;
        _filesProvider = filesProvider;
        _scheduler = scheduler;

        RegisterLoader<ITexture>(LoadTextureFunc);
        RegisterLoader<Sprite>(path => JsonSpriteLoader.Load(path, filesProvider));
    }

    public ResourceHandle<TResource> Get<TResource>(string path) where TResource : class, IDisposable
    {
        path = PathHelper.NormalizePath(path);
        
        var key = GetKey<TResource>(path);
        
        if (!_resources.TryGetValue(key, out var resource))
        {
            var obj = LoadResource<TResource>(path);

            resource = new ResourceInstance(obj);
            _resources.Add(key, resource);

            if (obj is IInstanceCountingResource icr)
            {
                icr.AddUser = g => resource.Users.Add(g);
                icr.RemoveUser = g =>
                {
                    resource.Users.Remove(g);
                    
                    if (resource.Users.Count == 0)
                    {
                        _resources.Remove(key);
                        _scheduler.Schedule(resource.Object.Dispose);
                    }
                };
            }
        }

        return FromResourceInstance<TResource>(key, resource);
    }

    private ResourceHandleManaged<TResource> FromResourceInstance<TResource>(string key, ResourceInstance resource)
        where TResource : class, IDisposable
    {
        var handle = new ResourceHandleManaged<TResource>((TResource)resource.Object, key, g =>
        {
            
            resource.Users.Remove(g);
            if (resource.Users.Count == 0)
            {
                resource.Object.Dispose();
                _resources.Remove(key);
            }
        }, () => FromResourceInstance<TResource>(key, resource));

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
                resource?.Dispose();
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
        var hasGlow = _filesProvider.FileExists(name + ".glow" + ext);

        return _ioCContainer.IoCConstruct<ITexture>(new LoadTextureParameters
        {
            DiffuseMapStream =  hasDiffuseImplicit ? _filesProvider.Open(name + ext) : hasDiffuseExplicit ? 
                _filesProvider.Open(name + ".diffuse" + ext) : null,
            
            NormalMapStream = hasNormals ? _filesProvider.Open(name + ".normal" + ext) : null,
            GlowMapStream = hasGlow ? _filesProvider.Open(name + ".glow" + ext) : null
        });
    }
}