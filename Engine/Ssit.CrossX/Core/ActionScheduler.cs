using System;
using System.Collections.Concurrent;

namespace Ssit.CrossX.Core;

internal class ActionScheduler: IActionScheduler
{
    private readonly ConcurrentQueue<Action> _actionQueue = new();

    public ActionScheduler(IEventSource eventSource)
    {
        eventSource.Updating += OnUpdating;
    }
    
    public void OnUpdating(float _)
    {
        while (_actionQueue.TryDequeue(out var action))
        {
            action();
        }
    }

    public void Schedule(Action action)
    {
        _actionQueue.Enqueue(action);
    }
}