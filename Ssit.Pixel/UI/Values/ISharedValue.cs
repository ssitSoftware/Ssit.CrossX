using System;

namespace Ssit.Pixel.UI.Values;

public interface ISharedValue<T>
{
    event Action<T> ValueChanged;
    T Value { get; set; }
}