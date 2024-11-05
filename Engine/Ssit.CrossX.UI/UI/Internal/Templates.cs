using System;
using System.Collections.Generic;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI.Internal;

public class Templates: ITemplates
{
    private readonly Dictionary<Type, TemplatesContainer> _containers = new();
    
    public TContainer Get<TContainer>() where TContainer : TemplatesContainer
    {
        if (_containers.TryGetValue(typeof(TContainer), out var container))
        {
            return (TContainer) container;
        }

        throw new KeyNotFoundException();
    }

    public void Register<TContainer>(TContainer container) where TContainer : TemplatesContainer
    {
        _containers.Add(typeof(TContainer), container);
    }
}