using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Services;

namespace Nokemono.Core.UI.ViewModels;

public class StartupPageViewModel
{
    public StartupPageViewModel(INavigation navigation, IActionScheduler actionScheduler)
    {
        Task.Delay(5000).ContinueWith(t =>
        {
            actionScheduler.Schedule(() =>
            {
                navigation.NavigateTo<MainPageViewModel>();
            });
        });
    }
}