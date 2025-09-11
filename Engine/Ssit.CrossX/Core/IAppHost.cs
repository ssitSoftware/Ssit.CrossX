using System;
using System.Numerics;

namespace Ssit.CrossX.Core;

public interface IAppHost : IDisposable
{
    void Resize(Size size, bool forceRecreation = false);
    void Render(object state, Action<object> renderAction);
    public Matrix3x2 Transform { get; }
    public Matrix3x2 TransformInv { get; }
    Size TargetSize { get; }
    int Scale { get; }
}