namespace Ssit.CrossX.UI.Services;

public interface INavigation
{
    void ClearNavigateTo<TViewModel>(object parameter = null) where TViewModel : class;
    void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class;
    void NavigateBack();
    void NavigateBackTo<TViewModel>() where TViewModel : class;
    int NavigationStackCount { get; }
    bool ParallelTransitions { get; set; }
    bool IsOnTop(object vmOrPage);
}

public interface INavigationEventHandler
{
    void OnNavigatedBackTo();
}