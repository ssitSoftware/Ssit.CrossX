using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ssit.CrossX.UI.Services;

internal class NavigationMap : INavigationMap
{
    private readonly Dictionary<Type, Type> _vmToPageMappings = new();

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
    public Type GetPageTypeFromViewModel(object viewModel)
    {
        var type = viewModel.GetType();
        if (!_vmToPageMappings.TryGetValue(type, out var result))
        {
            throw new InvalidOperationException();
        }

        // The dictionary is only populated via Map<TViewModel, TPage>() where TPage is annotated
        // with [DynamicallyAccessedMembers(PublicConstructors)], so constructors are preserved.
        return result;
    }

    public INavigationMap Map<TViewModel, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TPage>()
        where TViewModel : class where TPage : Page<TViewModel>
    {
        _vmToPageMappings.Add(typeof(TViewModel), typeof(TPage));
        return this;
    }
}