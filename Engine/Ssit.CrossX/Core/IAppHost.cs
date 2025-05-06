using System;
using System.Numerics;

namespace Ssit.CrossX.Core;

public interface IAppHost : IDisposable
{
    void Resize(Size size);
    void Render(object state, Action<object> renderAction);
    public Matrix3x2 Transform { get; }
    Size TargetSize { get; }
    int Scale { get; }
}