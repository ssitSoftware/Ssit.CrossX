using System;
using Ssit.CrossX.Content;
using Ssit.IoC;

namespace Ssit.CrossX.UI.Values;

public interface IImageSource<TResource> : IDisposable where TResource: class, IDisposable
{
    event Action ImageChanged;
    ResourceHandle<TResource> GetImage(IIoCContainer container);
    Rectangle? SourceRect { get; }
}