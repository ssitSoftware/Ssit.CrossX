using System.Collections.Generic;

namespace Ssit.CrossX.UI.Services;

public interface INavigation
{
    void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class;
    void NavigateBack();
    void NavigateBackTo<TViewModel>() where TViewModel : class;
    int NavigationStackCount { get; }
}