using System;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.IoC;

namespace Ssit.CrossX.UI.Values;

public interface IImageSource: IDisposable
{
    event Action ImageChanged;
    ResourceHandle<ITexture> GetTexture(IIoCContainer container);
    Rectangle? SourceRect { get; }
}