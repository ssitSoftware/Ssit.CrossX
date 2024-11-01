using System;

namespace Ssit.CrossX.UI.Values;

public interface ISharedValue<T>
{
    event Action<T> ValueChanged;
    T Value { get; set; }
}