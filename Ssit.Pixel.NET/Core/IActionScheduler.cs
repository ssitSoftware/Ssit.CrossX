using System;

namespace Ssit.Pixel.NET.Core;

public interface IActionScheduler
{
    void Schedule(Action action);
}