using System;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Services;

public interface IHandlerMapper
{
    IHandlerMapper AddMapping<TView, TViewHandler>() where TView : View where TViewHandler : ViewHandler<TView>;
    Type GetMapping(Type viewType);
    ViewHandler Create(View view, IViewParent parent);
}