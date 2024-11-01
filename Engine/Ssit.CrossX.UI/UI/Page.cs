namespace Ssit.CrossX.UI;

public abstract class Page<TViewModel>: View, IPage where TViewModel: class
{
    protected TViewModel ViewModel { get; private set; }
    protected IStylesManager Styles { get; private set; }
        
    void IPage.Load(IStylesManager styles, object viewModel)
    {
        ViewModel = (TViewModel)viewModel;
        Styles = styles;
    }

    protected abstract View CreateView();
}