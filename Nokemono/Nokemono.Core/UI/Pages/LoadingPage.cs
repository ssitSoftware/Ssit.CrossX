using Nokemono.Core.UI.ViewModels;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Pages;

internal class LoadingPage: Page<LoadingPageViewModel>
{
    private float _loadingDelay = 0.2f;
    
    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        if (_loadingDelay > 0)
        {
            if (TransitionProgress == 0)
            {
                _loadingDelay -= dt;

                if (_loadingDelay <= 0)
                {
                    ViewModel.StartLoading();
                }
            }
        }
    }

    protected override View CreateView()
    {
        return new Container
        {
            BackgroundColor = Palette.Background,
            Children = []
        };
    }
}