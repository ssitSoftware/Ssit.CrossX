using System;

namespace Ssit.CrossX.Content;

public abstract class ResourceHandle<TResource> : IDisposable where TResource : class, IDisposable
{
    /// <summary>
    /// Resource reference.
    /// </summary>
    public TResource Resource { get; }
    
    /// <summary>
    /// Resource name.
    /// </summary>
    public string Name { get; }
    
    internal ResourceHandle(TResource resource, string name)
    {
        Resource = resource;
        Name = name;
    }

    public void Dispose()
    {
        OnDispose();
    }

    protected abstract void OnDispose();
}

/// <summary>
/// Represents disposable handle to the resource. 
/// </summary>
/// <typeparam name="TResource">Type of the underlying resource.</typeparam>
public class ResourceHandleManaged<TResource> : ResourceHandle<TResource> where TResource : class, IDisposable
{
    internal Guid Guid { get; } = Guid.NewGuid();

    private readonly Action<Guid> _onDispose;
    
    internal ResourceHandleManaged(TResource resource, string name, Action<Guid> onDispose):
        base(resource, name)
    {
        _onDispose = onDispose;
    }
    
    /// <summary>
    /// Disposes resource handle.
    /// </summary>
    protected override void OnDispose()
    {
        _onDispose?.Invoke(Guid);
    }
}

public class ResourceHandleUnmanaged<TResource>(TResource resource, string name)
    : ResourceHandle<TResource>(resource, name)
    where TResource : class, IDisposable
{
    private bool _isDisposed;

    protected override void OnDispose()
    {
        if (_isDisposed)
            return;
        
        Resource?.Dispose();
        _isDisposed = true;
    }
}