using System;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Core;

public interface IGameInstance: IDisposable
{
    int RenderPasses { get; }
    void Render(IRenderer2 renderer, RectangleF target, int renderPass, float scale);
    void RenderDebug(IRenderer2 renderer, RectangleF target, float scale);
    void Update(float deltaTime);
}