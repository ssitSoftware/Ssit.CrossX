using System;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.XxFormats.Editor;
using Ssit.CrossX.XxGames.Platformer.Builders;

namespace Ssit.CrossX.XxGames.Logic.Objects;

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