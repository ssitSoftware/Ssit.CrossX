using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ssit.CrossX.Core;

[SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ParametersBase
{
    public event Action ApplyParameters;

    protected void SetField<T>(ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        HasChanges = true;
    }

    public void Apply(bool raiseEvent = true)
    {
        if (raiseEvent)
        {
            ApplyParameters?.Invoke();
        }

        HasChanges = false;
    }
        
    public bool HasChanges { get; private set; }
}