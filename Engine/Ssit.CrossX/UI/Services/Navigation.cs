using System;
using System.Collections.Generic;
using System.Reflection;
using Ssit.IoC;

namespace Ssit.CrossX.UI.Services;

internal class Navigation: INavigation
{
    private class StackData
    {
        public object ViewModel;
        public string FocusedId;

        public StackData(object viewModel)
        {
            ViewModel = viewModel;
            FocusedId = null;
        }
    }
    
    private readonly NavigationMap _navigationMap;
    private readonly IIoCContainer _iocContainer;
    private readonly IUiServices _uiServices;
    private readonly UiApp _uiApp;

    private readonly Stack<StackData> _navigationStack = new();
    private readonly List<IDisposable> _objectsToDisposeOnTransitionFinished = new();
    
    public IPage PreviousPage { get; private set; }
    public IPage CurrentPage { get; private set; }
    public bool PreviousPageOnTop { get; private set; }
    
    public Navigation(NavigationMap navigationMap, IIoCContainer iocContainer, IUiServices uiServices, IUiApp uiApp)
    {
        _navigationMap = navigationMap;
        _iocContainer = iocContainer;
        _uiServices = uiServices;
        _uiApp = uiApp as UiApp;
    }

    public int NavigationStackCount => _navigationStack.Count;
    
    public void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class
    {
        if (_navigationStack.Count > 0)
        {
            var data = _navigationStack.Peek();
            data.FocusedId = CurrentPage.FocusedElement?.UniqueId;
        }
        
        var vm = _iocContainer.IoCConstruct<TViewModel>(parameter);
        _navigationStack.Push(new StackData(vm));
        
        var page = InitializePage(vm, null);
        PreviousPage = CurrentPage;
        CurrentPage = page;

        if (PreviousPage != null)
        {
            PreviousPage.TransitionProgress = 1;
        }
        CurrentPage.TransitionProgress = 1;
        
        PreviousPageOnTop = false;
    }

    public void NavigateBack()
    {
        if (_navigationStack.Count == 1)
        {
            throw new InvalidOperationException("Cannot navigate back when the navigation stack is empty.");
        }

        var oldData = _navigationStack.Pop();

        if (oldData.ViewModel is IDisposable disposable)
        {
            _objectsToDisposeOnTransitionFinished.Add(disposable);
        }

        while (_navigationStack.Peek().ViewModel.GetType().GetCustomAttribute(typeof(NoHistoryAttribute)) != null)
        {
            (_navigationStack.Pop().ViewModel as IDisposable)?.Dispose();
        }

        var data = _navigationStack.Peek();
        var page = InitializePage(data.ViewModel, data.FocusedId);
        
        PreviousPage = CurrentPage;
        CurrentPage = page;
        PreviousPageOnTop = true;
        
        if (PreviousPage != null)
        {
            PreviousPage.TransitionProgress = 1;
        }
        CurrentPage.TransitionProgress = 1;
    }

    public void NavigateBackTo<TViewModel>() where TViewModel : class
    {
        if (_navigationStack.Count == 1)
        {
            throw new InvalidOperationException("Cannot navigate back when the navigation stack is empty.");
        }
        
        var oldData = _navigationStack.Pop();
        
        if (oldData.ViewModel is IDisposable disposable2)
        {
            _objectsToDisposeOnTransitionFinished.Add(disposable2);
        }
        
        var data = _navigationStack.Peek();
        
        while (_navigationStack.Count > 1)
        {
            if (data.ViewModel is TViewModel) break;

            if (data.ViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _navigationStack.Pop();
            data = _navigationStack.Peek();
        }
        
        var page = InitializePage(data.ViewModel, data.FocusedId);
        
        PreviousPage = CurrentPage;
        CurrentPage = page;
        PreviousPageOnTop = true;
        
        if (PreviousPage != null)
        {
            PreviousPage.TransitionProgress = 1;
        }
        CurrentPage.TransitionProgress = 1;
    }

    private IPage InitializePage(object vm, string focusedId)
    {
        var pageType = _navigationMap.GetPageTypeFromViewModel(vm);
        
        var page = (IPage)Activator.CreateInstance(pageType);
        if (page is null) throw new InvalidProgramException();
        
        page.Load(_uiServices, _uiApp.InputProcessor, vm);
        page.SetBounds(_uiApp.Bounds, _uiApp.Scale);
        
        if (focusedId != null)
        {
            var focusable = _uiApp.InputProcessor.FindFocusable(focusedId, page);
            if (focusable != null)
            {
                _uiApp.InputProcessor.Focus(focusable, page);
            }
        }
        
        return page;
    }

    public void Update(float dt)
    {
        if (PreviousPage?.TransitionProgress >= 1)
        {
            PreviousPage?.Dispose();
            PreviousPage = null;
        }
        
        PreviousPage?.Update(dt);
        CurrentPage?.Update(dt);

        if (PreviousPage is null)
        {
            foreach (var disposable in _objectsToDisposeOnTransitionFinished)
            {
                disposable.Dispose();
            }
            _objectsToDisposeOnTransitionFinished.Clear();
        }
    }
}