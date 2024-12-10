using System.Diagnostics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI;

public abstract class Page<TViewModel>: View, IPage, IViewParent where TViewModel: class
{
    protected TViewModel ViewModel { get; private set; }
    protected IStylesManager Styles { get; private set; }

    private IIoCContainer _iocContainer;
    private ViewHandler _rootHandler;
    private RectangleF _screenBounds;
    private bool _recalculateLayout;
    
    RectangleF IViewParent.ScreenBounds => _screenBounds;
    
    public void RecalculateLayout(View view = null)
    {
        Debug.Assert(view == null || view == _rootHandler.View);
        _recalculateLayout = true;
    }
    
    void IPage.Load(RectangleF bounds, IUiServices services, object viewModel)
    {
        ViewModel = (TViewModel)viewModel;
        Styles = services.StylesManager;
        _iocContainer = services.IoCContainer;

        var root = CreateView();
        _rootHandler = services.HandlerMapper.Create(root, this);
        
        _rootHandler.SetBounds(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));
        _screenBounds = bounds;
    }

    void IPage.Update(float dt)
    {
        if (_recalculateLayout)
        {
            _rootHandler.SetBounds(new RectangleF(0,0,_screenBounds.Width, _screenBounds.Height));
            _recalculateLayout = false;
        }
        
        OnUpdate(dt);
    }

    protected virtual void OnUpdate(float dt)
    {
        _rootHandler.Update(dt);
    }

    void IPage.Draw(IRenderer renderer) => OnDraw(renderer);
    
    void IPage.SetBounds(RectangleF bounds)
    {
        _screenBounds = bounds;
        _rootHandler.SetBounds(new RectangleF(0,0,_screenBounds.Width, _screenBounds.Height));
    }

    protected virtual void OnDraw(IRenderer renderer)
    {
        _rootHandler.Draw(renderer);
    }
    
    protected TContainer GetContainer<TContainer>() where TContainer : TemplatesContainer
    {
        return _iocContainer.Get<TContainer>();
    }
    
    protected abstract View CreateView();
    
}