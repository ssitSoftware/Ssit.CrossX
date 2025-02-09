using Ssit.CrossX.Core;

namespace Ssit.CrossX.SDL3.Services;

internal class LifeTimeManager: IAppLifetimeManager
{
    public bool ShouldContinue { get; private set; } = true;
    
    public event Action AppExiting; 
    
    public void Exit()
    {
        ShouldContinue = false;
    }
    
    public void RaiseAppExiting()
    {
        AppExiting?.Invoke();
    }
}