using System;
using System.Collections.Generic;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.UI.Services;

internal class Navigation: INavigation
{
    private readonly NavigationMap _navigationMap;
    private readonly IIoCContainer _iocContainer;
    private readonly IUiServices _uiServices;
    private readonly IUiAppBoundsSource _uiAppBoundsSource;

    private readonly Stack<object> _navigationStack = new();
    private readonly List<IDisposable> _objectsToDisposeOnTransitionFinished = new();
    
    public IPage PreviousPage { get; private set; }
    public IPage CurrentPage { get; private set; }
    public bool PreviousPageOnTop { get; private set; }
    
    public Navigation(NavigationMap navigationMap, IIoCContainer iocContainer, IUiServices uiServices, IUiApp uiApp)
    {
        _navigationMap = navigationMap;
        _iocContainer = iocContainer;
        _uiServices = uiServices;
        _uiAppBoundsSource = uiApp as IUiAppBoundsSource;
    }
    
    public void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class
    {
        var vm = _iocContainer.IoCConstruct<TViewModel>(parameter);
        _navigationStack.Push(vm);
        
        var page = InitializePage(vm);
        PreviousPage = CurrentPage;
        CurrentPage = page;
        PreviousPageOnTop = false;
    }

    public void NavigateBack()
    {
        if (_navigationStack.Count == 1)
        {
            throw new InvalidOperationException("Cannot navigate back when the navigation stack is empty.");
        }
        
        var oldVm = _navigationStack.Pop();
        
        if (oldVm is IDisposable disposable)
        {
            _objectsToDisposeOnTransitionFinished.Add(disposable);
        }
        
        var vm = _navigationStack.Peek();
        var page = InitializePage(vm);
        
        PreviousPage = CurrentPage;
        CurrentPage = page;
        PreviousPageOnTop = true;
    }

    public void NavigateBackTo<TViewModel>() where TViewModel : class
    {
        if (_navigationStack.Count == 1)
        {
            throw new InvalidOperationException("Cannot navigate back when the navigation stack is empty.");
        }
        
        var oldVm = _navigationStack.Pop();
        
        if (oldVm is IDisposable disposable2)
        {
            _objectsToDisposeOnTransitionFinished.Add(disposable2);
        }
        
        var vm = _navigationStack.Peek();
        
        while (_navigationStack.Count > 1)
        {
            if (vm is TViewModel) break;

            if (vm is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _navigationStack.Pop();
            vm = _navigationStack.Peek();
        }
        
        var page = InitializePage(vm);
        
        PreviousPage = CurrentPage;
        CurrentPage = page;
        PreviousPageOnTop = true;
    }

    private IPage InitializePage(object vm)
    {
        var pageType = _navigationMap.GetPageTypeFromViewModel(vm);
        
        var page = (IPage)Activator.CreateInstance(pageType);
        if (page is null) throw new InvalidProgramException();
        
        page.Load(_uiAppBoundsSource.Bounds, _uiServices, vm);
        return page;
    }

    public void Update(float dt)
    {
        PreviousPage?.Update(dt);
        CurrentPage?.Update(dt);

        if (true)
        {
            foreach (var disposable in _objectsToDisposeOnTransitionFinished)
            {
                disposable.Dispose();
            }
        }
    }
}