using System;

namespace Ssit.Pixel.Content;

/// <summary>
/// Represents disposable handle to the resource. 
/// </summary>
/// <typeparam name="TResource">Type of the underlying resource.</typeparam>
public class ResourceHandle<TResource> : IDisposable where TResource: class, IDisposable
{
    /// <summary>
    /// Resource reference.
    /// </summary>
    public TResource Resource { get; }
    
    /// <summary>
    /// Resource name.
    /// </summary>
    public string Name { get; }
    
    internal Guid Guid { get; } = Guid.NewGuid();

    private readonly Action<Guid> _onDispose;
    
    internal ResourceHandle(TResource resource, string name, Action<Guid> onDispose)
    {
        _onDispose = onDispose;
        Resource = resource;
        Name = name;
    }
    
    /// <summary>
    /// Disposes resource handle.
    /// </summary>
    public void Dispose()
    {
        _onDispose?.Invoke(Guid);
    }
}