using System;

namespace Ssit.CrossX.UI.Services;

public interface IUiActionDispatcher
{
    void Enqueue(Action action);
}