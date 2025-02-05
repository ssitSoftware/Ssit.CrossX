using System;
using System.Numerics;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.Core;

public interface IAppHost : IDisposable
{
    void Resize(Size size);
    void Render(Action<RenderMode> renderAction);
    public Matrix3x2 Transform { get; }
    Size TargetSize { get; }
    int Scale { get; }
}