using System;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.Core;

public interface ISimulation: IDisposable
{
    int RenderPasses { get; }
    void Render(IRenderer renderer, RectangleF target, int renderPass, float scale);
    void Update(float deltaTime);
}