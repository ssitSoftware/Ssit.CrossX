using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Nokemono.Core.UI.ViewModels;

public class StartupPageViewModel
{
    public SharedBoolMutable ShowPoweredBy { get; } = new SharedBoolMutable(false);
    
    public StartupPageViewModel(INavigation navigation, IActionScheduler actionScheduler)
    {
        Task.Delay(1200).ContinueWith(t =>
        {
            ShowPoweredBy.SetValue(true);
        });
        
        Task.Delay(1800).ContinueWith(t =>
        {
            actionScheduler.Schedule(() =>
            {
                navigation.NavigateTo<MainPageViewModel>();
            });
        });
    }
}