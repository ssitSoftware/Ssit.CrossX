using System;
using System.Collections.Generic;

namespace Ssit.CrossX.UI.Services;

internal class NavigationMap : INavigationMap
{
    private readonly Dictionary<Type, Type> _vmToPageMappings = new();

    public Type GetPageTypeFromViewModel(object viewModel)
    {
        var type = viewModel.GetType();
        if (!_vmToPageMappings.TryGetValue(type, out var result))
        {
            throw new InvalidOperationException();
        }

        return result;
    }
    
    public INavigationMap Map<TViewModel, TPage>() where TViewModel : class where TPage : Page<TViewModel>
    {
        _vmToPageMappings.Add(typeof(TViewModel), typeof(TPage));
        return this;
    }
}