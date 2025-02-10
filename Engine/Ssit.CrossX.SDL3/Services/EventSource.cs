using Ssit.CrossX.Core;

namespace Ssit.CrossX.SDL3.Services;

internal class EventSource: IEventSource
{
    public event Action<float> Updating;
    public event Action Updated;
    public event Action RenderFinished;
    
    public void OnUpdate( float deltaTime )
    {
        Updating?.Invoke(deltaTime);
    }

    public void OnUpdated()
    {
        Updated?.Invoke();
    }
    
    public void OnRenderFinished()
    {
        RenderFinished?.Invoke();
    }
}