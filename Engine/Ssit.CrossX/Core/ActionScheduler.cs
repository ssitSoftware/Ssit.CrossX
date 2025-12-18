using System;
using System.Collections.Concurrent;
using Ssit.CrossX.Core.Internal;

namespace Ssit.CrossX.Core;

internal class ActionScheduler: IInternalActionScheduler
{
    private readonly ConcurrentQueue<Action> _actionQueue = new();

    public ActionScheduler(IEventSource eventSource) => eventSource.Updating += OnUpdating;

    private void OnUpdating(float _) => Process();

    public void Schedule(Action action) => _actionQueue.Enqueue(action);

    public void Process()
    {
        while (_actionQueue.TryDequeue(out var action))
        {
            action();
        }
    }
}