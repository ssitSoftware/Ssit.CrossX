using System;

namespace Ssit.CrossX.Games.Logic.Objects;

public interface ISwitch
{
    event Action OnChanged;
    bool IsOn { get; }
    void Toggle();
}