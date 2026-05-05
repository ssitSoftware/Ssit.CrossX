using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Services;

public interface IHandlerMapper
{
    IHandlerMapper AddMapping<TView, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewHandler>()
        where TView : View where TViewHandler : ViewHandler<TView>;

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    Type GetMapping(Type viewType);

    ViewHandler Create(View view, IViewParent parent);
}