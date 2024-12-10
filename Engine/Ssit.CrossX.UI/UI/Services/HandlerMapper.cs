using System;
using System.Collections.Generic;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Services;

internal class HandlerMapper : IHandlerMapper
{
    private readonly IIoCContainer _iocContainer;
    private Dictionary<Type, Type> _mappings = new();

    public HandlerMapper(IIoCContainer iocContainer)
    {
        _iocContainer = iocContainer;
    }
    
    public IHandlerMapper AddMapping<TView, TViewHandler>() where TView : View where TViewHandler : ViewHandler<TView>
    {
        _mappings[typeof(TView)] = typeof(TViewHandler);
        return this;
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

    public ViewHandler Create(View view, IViewParent parent = null)
    {
        var type = GetMapping(view.GetType());
        return (ViewHandler)_iocContainer.IoCConstruct(type, new ViewHandler.CreateHandlerParameters
        {
            View = view,
            Parent = parent
        });
    }
}