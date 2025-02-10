using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Core;

public interface ISimulation: IDisposable
{
    int RenderPasses { get; }
    void Render(IRenderer2 renderer, RectangleF target, int renderPass, float scale);
    void Update(float deltaTime);
}