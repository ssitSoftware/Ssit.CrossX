using System;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.IoC;

namespace Ssit.CrossX.Core;

public interface IGameInstance: IDisposable
{
    IMessenger Messenger { get; }
    event Action<float> FixedUpdate;
    int RenderPasses { get; }
    void Render(IRenderer2 renderer, RectangleF target, int renderPass, float scale);
    void RenderDebug(IRenderer2 renderer, RectangleF target, float scale);
    void Update(float deltaTime);
    TService GetComponent<TService>() where TService : class;
    void Activate(bool active);
    IIoCContainer Services { get; }
}