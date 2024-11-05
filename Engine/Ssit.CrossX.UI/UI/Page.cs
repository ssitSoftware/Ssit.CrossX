using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI;

public abstract class Page<TViewModel>: View, IPage where TViewModel: class
{
    protected TViewModel ViewModel { get; private set; }
    protected IStylesManager Styles { get; private set; }

    private IIoCContainer _iocContainer;
    
    void IPage.Load(IUiServices services, object viewModel)
    {
        ViewModel = (TViewModel)viewModel;
        Styles = services.StylesManager;
        _iocContainer = services.IoCContainer;
    }

    protected TContainer GetContainer<TContainer>() where TContainer : TemplatesContainer
    {
        return _iocContainer.Get<TContainer>();
    }
    
    protected abstract View CreateView();
}