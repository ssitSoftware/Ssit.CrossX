using System;
using System.Diagnostics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI;

public abstract class Page<TViewModel>: View, IPage where TViewModel: class
{
    protected TViewModel ViewModel { get; private set; }
    protected IStylesManager Styles { get; private set; }

    private IIoCContainer _iocContainer;
    private ViewHandler _rootHandler;
    private RectangleF _screenBounds;
    private bool _recalculateLayout;
    
    protected IFocusable FocusedElement { get; private set; }

    public float TransitionTime { get; set; } = 0;
    public float TransitionProgress { get; set; }

    protected SizeF ScreenSize => _screenBounds.Size;
    
    IFocusable IPage.FocusedElement
    {
        get => FocusedElement;
        set => FocusedElement = value;
    }

    float IPage.Scale => Scale;
    protected virtual float Scale => 1;
    
    RectangleF IViewParent.ScreenBounds => _screenBounds;
    
    ViewHandler IPage.RootHandler => _rootHandler;

    TParent IViewParent.GetParent<TParent>()
    {
        if (this is TParent parent)
        {
            return parent;
        }

        throw new NotSupportedException();
    }
    
    RectangleF IViewParent.CalculateTargetBounds()
    {
        return _screenBounds;
    }
    
    public void RecalculateLayout(View view = null)
    {
        Debug.Assert(view == null || view == _rootHandler.View);
        _recalculateLayout = true;
    }
    
    void IPage.Load(RectangleF bounds, IUiServices services, IInputContext inputContext, object viewModel)
    {
        ViewModel = (TViewModel)viewModel;
        Styles = services.StylesManager;
        _iocContainer = services.IoCContainer;

        var root = CreateView();
        _rootHandler = services.HandlerMapper.Create(root, this);
        
        _screenBounds = bounds;
        
        _rootHandler.SetBounds(new RectangleF(0, 0, bounds.Width, bounds.Height));
        _recalculateLayout = true;
        
        OnLoad(inputContext);
    }

    protected virtual void OnLoad(IInputContext inputContext)
    {
        
    }

    void IPage.Update(float dt)
    {
        if (_recalculateLayout)
        {
            _rootHandler.SetBounds(new RectangleF(0, 0,_screenBounds.Width, _screenBounds.Height));
            _recalculateLayout = false;
        }
        
        OnUpdate(dt);
    }

    bool IPage.OnUiButton(UiButton button, IInputContext inputContext) => OnUiButton(button, inputContext);

    protected virtual void OnUpdate(float dt)
    {
        _rootHandler.Update(dt);
    }

    protected virtual bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (button is UiButton.Back or UiButton.MenuOrBack)
        {
            return OnBack();
        }

        return false;
    }

    protected virtual bool OnBack()
    {
        if (ViewModel is IBackCommandSource bcs && (bcs.BackCommand?.CanExecute(null) ?? false))
        {
            bcs.BackCommand.Execute(null);
            return true;
        }
        
        return false;
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

    void IDisposable.Dispose()
    {
        _rootHandler.Dispose();
    }
}