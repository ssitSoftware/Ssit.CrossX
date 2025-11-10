using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.XxFormats.Editor;

namespace Ssit.CrossX.Games.Logic.Objects;

public sealed class VirtualSwitch(ObjectCreationParameters<VirtualSwitch.Parameters> parameters) : ISwitch
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Parameters
    {
        [Editor] public bool IsOn { get; set; }
    }
    
    public event Action OnChanged;
    public bool IsOn { get; private set; } = parameters.Parameters.IsOn;

    public void Toggle()
    {
        IsOn = !IsOn;
        OnChanged?.Invoke();
    }
}