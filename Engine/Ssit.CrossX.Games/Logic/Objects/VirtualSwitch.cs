using System;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic.Map;

namespace Ssit.CrossX.Games.Logic.Objects;

public sealed class VirtualSwitch(ObjectCreationParameters<VirtualSwitch.Parameters> parameters) : ISwitch
{
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