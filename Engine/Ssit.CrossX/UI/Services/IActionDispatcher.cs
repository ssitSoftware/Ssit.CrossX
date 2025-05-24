using System;

namespace Ssit.CrossX.UI.Services;

public interface IActionDispatcher
{
    void Enqueue(Action action);
}