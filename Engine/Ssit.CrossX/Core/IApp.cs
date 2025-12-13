using System;
using System.Runtime.CompilerServices;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.IoC;

namespace Ssit.CrossX.Core;

public interface IApp: IDisposable
{
    bool IsActive { get; }
    void InitializeServices(IIoCContainerBuilder builder);
    void SetActive(bool active);

    void Update(float dt);
    void Draw(IRenderer2 renderer);

    void Resize(Size size);
    void Start(object args);
    void Initialize(IIoCContainer container);
}