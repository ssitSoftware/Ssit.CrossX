using System;
using System.Collections.Generic;

namespace Ssit.CrossX.UI.Services;

internal class ActionDispatcher : IActionDispatcher
{
    private object _lock = new();
    
    private readonly List<Action> _actions = new();
    
    public void Dispatch()
    {
        lock (_lock)
        {
            foreach (var action in _actions)
            {
                action();
            }
        }
    }
    
    public void Enqueue(Action action)
    {
        lock (_lock)
        {
            _actions.Add(action);
        }
    }
}