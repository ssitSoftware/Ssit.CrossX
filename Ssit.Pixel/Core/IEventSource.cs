using System;

namespace Ssit.Pixel.Core;

public interface IEventSource
{
    event Action<float> Updating;
    event Action Updated;
    event Action RenderFinished;
}