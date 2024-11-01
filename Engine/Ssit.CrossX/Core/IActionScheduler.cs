using System;

namespace Ssit.CrossX.Core;

public interface IActionScheduler
{
    void Schedule(Action action);
}