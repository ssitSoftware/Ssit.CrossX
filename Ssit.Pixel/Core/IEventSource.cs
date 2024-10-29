using System;

namespace Ssit.Pixel.Core;

public interface IEventSource
{
    event Action Updating;
    event Action Updated;
    event Action RenderFinished;
}