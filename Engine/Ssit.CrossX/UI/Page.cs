using System;
using System.Diagnostics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.IoC;
using Ssit.CrossX.UI.Exceptions;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Transitions;
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
    private bool _recalculationNeeded;
    private bool _renderingInvalid;
    
    private RectangleF _bounds;
    private float _transitionProgress;
    protected IIoCContainer Services => _iocContainer;
    
    protected IFocusable FocusedElement { get; private set; }

    public virtual float TransitionTime => 0f;
    
    float IPage.TransitionProgress { get => _transitionProgress; set => _transitionProgress = value; }

    protected bool IsInTransition => _transitionProgress > 0;
    
    protected SizeF ScreenSize => _screenBounds.Size;
    
    IFocusable IPage.FocusedElement
    {
        get => FocusedElement;
        set => FocusedElement = value;
    }

    void IPage.InvalidateRendering()
    {
        _renderingInvalid = true;
    }

    float IPage.Scale => Scale;
    TransitionType IPage.TransitionType { get; set; }
    
    void IPage.SignalRecalculationPending() => _recalculationNeeded = true;

    protected virtual float Scale { get; private set; } = 1;
    
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
        return _bounds;
    }

    void IPage.OnTransitionToFinished() => OnTransitionToFinished();

    protected virtual void OnTransitionToFinished()
    {
    }
    
    public void RecalculateLayout(View view = null)
    {
        Debug.Assert(view == null || view == _rootHandler.View);
        _recalculateLayout = true;

        if (view is IViewParent parent)
        {
            parent.RecalculateLayout();
        }
        _recalculationNeeded = true;
    }
    
    void IPage.Load(IUiServices services, IInputContext inputContext, object viewModel)
    {
        ViewModel = (TViewModel)viewModel;
        Styles = services.StylesManager;
        _iocContainer = services.IoCContainer;

        var root = CreateView();
        _rootHandler = services.HandlerMapper.Create(root, this);
        OnLoad(inputContext);
        _recalculateLayout = true;
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

        while (_recalculationNeeded)
        {
            _recalculationNeeded = false;
            _rootHandler.Update(0);
        }
    }

    bool IPage.OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (_transitionProgress > 0)
            return false;
        
        return OnUiButton(button, inputContext);
    }

    protected virtual void OnUpdate(float dt)
    {
        _rootHandler.Update(dt);
    }

    protected virtual bool OnUiButton(UiButton button, IInputContext inputContext)
    {
        if (ViewModel is IUiButtonHandler handler)
        {
            if (handler.OnUiButton(button))
            {
                return true;
            }
        }
        
        switch (button)
        {
            case UiButton.Back:
                return OnBack();
            
            case UiButton.Menu:
                return OnMenu();
            
            case UiButton.MenuOrBack:
                return OnMenu() || OnBack();
        }

        return false;
    }

    protected virtual bool OnBack()
    {
        if (ViewModel is IPageCommandsSource bcs && (bcs.BackCommand?.CanExecute(null) ?? false))
        {
            bcs.BackCommand.Execute(null);
            return true;
        }
        
        return false;
    }
    
    protected virtual bool OnMenu()
    {
        if (ViewModel is IPageCommandsSource bcs && (bcs.MenuCommand?.CanExecute(null) ?? false))
        {
            bcs.MenuCommand.Execute(null);
            return true;
        }
        
        return false;
    }

    void IPage.Draw(IRenderer2 renderer) => OnDraw(renderer);
    
    void IPage.SetBounds(RectangleF bounds, float scale) => SetBounds(bounds, scale);

    private void SetBounds(RectangleF bounds, float scale)
    {
        Scale = scale;

        _bounds = _screenBounds = new RectangleF(bounds.X * scale, bounds.Y * scale, bounds.Width * scale, bounds.Height * scale);
        _rootHandler.SetBounds(new RectangleF(0,0,_bounds.Width, _bounds.Height));
        
        _recalculationNeeded = true;
        while (_recalculationNeeded)
        {
            _recalculationNeeded = false;
            _rootHandler.Update(0);
        }
    }

    protected virtual void OnDraw(IRenderer2 renderer)
    {
        _rootHandler.Draw(renderer);

        if (_renderingInvalid)
        {
            _renderingInvalid = false;
            throw new InvalidRenderingException();
        }
    }
    
    protected TContainer GetContainer<TContainer>() where TContainer : TemplatesContainer
    {
        return _iocContainer.Get<TContainer>();
    }

    protected abstract View CreateView();

    protected virtual void OnDispose(bool disposing)
    {
        if (disposing)
        {
            _rootHandler.Dispose();
        }
    }
    
    ~Page()
    {
        OnDispose(false);
    }
    
    void IDisposable.Dispose()
    {
        OnDispose(true);
    }
}