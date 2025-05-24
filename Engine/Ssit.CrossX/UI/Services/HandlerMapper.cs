using System;
using System.Collections.Generic;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Services;

internal class HandlerMapper : IHandlerMapper
{
    public IReadOnlyDictionary<Type, Type> Mappings => _mappings;
    private readonly Dictionary<Type, Type> _mappings = new();
    
    public IHandlerMapper AddMapping<TView, TViewHandler>() where TView : View where TViewHandler : ViewHandler<TView>
    {
        _mappings[typeof(TView)] = typeof(TViewHandler);
        return this;
    }

    protected void AddMapping(Type viewType, Type handlerType)
    {
        _mappings[viewType] = handlerType;
    }

    public Type GetMapping(Type viewType)
    {
        var type = viewType;
        
        while (type != null && type != typeof(View))
        {
            if (_mappings.TryGetValue(viewType, out Type handlerType))
            {
                return handlerType;
            }
            type = type.BaseType;
        }

        throw new KeyNotFoundException($"No handler registered for {viewType}");
    }

    public virtual ViewHandler Create(View view, IViewParent parent)
    {
        throw new NotSupportedException();
    }
}