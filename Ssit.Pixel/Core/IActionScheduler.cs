using System;

namespace Ssit.Pixel.Core;

public interface IActionScheduler
{
    void Schedule(Action action);
}