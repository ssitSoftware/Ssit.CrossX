using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI;

namespace Nokemono.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Possible")]
[NoHistory]
internal class LoadingPageViewModel
{
    public class Parameters
    {
        public Action OnLoaded { get; set; }
        public Action OnLoading { get; set; }
    }
    
    private readonly Parameters _parameters;
    private readonly IActionScheduler _actionScheduler;

    public LoadingPageViewModel(Parameters parameters, IActionScheduler actionScheduler)
    {
        _parameters = parameters;
        _actionScheduler = actionScheduler;
    }

    public void StartLoading()
    { 
        _parameters.OnLoading();
        _actionScheduler.Schedule(_parameters.OnLoaded);
    }
}