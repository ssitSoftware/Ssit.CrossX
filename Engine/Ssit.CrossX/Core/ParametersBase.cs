using System;
using System.Collections.Generic;

namespace Ssit.CrossX.Core;

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