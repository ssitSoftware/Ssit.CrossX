using System;
using System.Collections.Concurrent;
using System.Threading;
using Ssit.CrossX.Core.Internal;

namespace Ssit.CrossX.Core;

internal class ActionScheduler: IInternalActionScheduler
{
    private readonly ConcurrentQueue<Action> _actionQueue = new();

    private int _mainThreadId;

    public ActionScheduler(IEventSource eventSource)
    {
        eventSource.Updating += OnUpdating;
        _mainThreadId = Thread.CurrentThread.ManagedThreadId;
    }

    private void OnUpdating(float _) => Process();

    public void Schedule(Action action) => _actionQueue.Enqueue(action);

    public void ExecuteOnMainThread(Action action)
    {
        var currId = Thread.CurrentThread.ManagedThreadId;

        if (currId == _mainThreadId) action?.Invoke();
        else Schedule(action);
    }
    
    public void Process()
    {
        while (_actionQueue.TryDequeue(out var action))
        {
            action();
        }
    }
}