using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI;

namespace Nokemono.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Possible")]
[NoHistory]
internal class LoadingPageViewModel: IDisposable
{
    public class Parameters
    {
        public Action OnLoaded { get; set; }
        public Action OnLoading { get; set; }
    }
    
    private readonly IEventSource _eventSource;
    private readonly Parameters _parameters;
    
    public LoadingPageViewModel(IEventSource eventSource, Parameters parameters)
    {
        _eventSource = eventSource;
        _parameters = parameters;
        eventSource.RenderFinished += EventSourceOnRenderFinished;
    }

    private void EventSourceOnRenderFinished()
    { 
        _eventSource.RenderFinished -= EventSourceOnRenderFinished;
        
        _parameters.OnLoading();
        _parameters.OnLoaded();
    }

    public void Dispose()
    {
        _eventSource.RenderFinished -= EventSourceOnRenderFinished;
    }
}