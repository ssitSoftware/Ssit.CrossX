namespace Ssit.Pixel.UI;

public abstract class Page<TViewModel>: View, IPage where TViewModel: class
{
    protected TViewModel ViewModel { get; private set; }

    void IPage.Load(object viewModel)
    {
        ViewModel = (TViewModel)viewModel;
    }

    protected abstract View CreateView();
}