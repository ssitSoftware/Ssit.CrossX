using System;
using System.Collections.Generic;
using Ssit.Pixel.Framework.IO;
using Ssit.Utils.IoC;

namespace Ssit.Pixel.Framework.Content.Internal;

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
    }
    
    public ResourceHandle<TResource> Load<TResource>(string path) where TResource : class, IDisposable
    {
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
}