using System;

namespace Ssit.CrossX.Core;

public interface IEventSource
{
    event Action<float> Updating;
    event Action Updated;
    event Action RenderFinished;

    event Action Paused;
    event Action Resumed;
}