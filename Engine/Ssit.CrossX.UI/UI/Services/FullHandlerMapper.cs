using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Services;

internal class FullHandlerMapper(IIoCContainer iocContainer) : HandlerMapper
{
    public void AddMappings(HandlerMapper mapper)
    {
        foreach(var pair in mapper.Mappings)
        {
            AddMapping(pair.Key, pair.Value);
        }
    }
    
    public override ViewHandler Create(View view, IViewParent parent)
    {
        var type = view.CustomHandlerType ?? GetMapping(view.GetType());
        
        var handler = (ViewHandler)iocContainer.IoCConstruct(type, new ViewHandler.CreateHandlerParameters
        {
            View = view,
            Parent = parent
        });
        
        handler.Init();
        return handler;
    }
}