namespace Ssit.CrossX.UI.Services;

public interface INavigationMap
{
    INavigationMap Map<TViewModel, TPage>() where TViewModel : class where TPage : Page<TViewModel>;
}