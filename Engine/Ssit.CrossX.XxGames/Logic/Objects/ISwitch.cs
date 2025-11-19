using System;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface ISwitch
{
    event Action OnChanged;
    bool IsOn { get; }
    void Toggle();
}