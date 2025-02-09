using System;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Core;

public interface IApp: IDisposable
{
    bool IsActive { get; }
    void InitializeServices(IIoCContainerBuilder builder);
    void SetActive(bool active);
    void Update(Action<float> preUpdate);
    void Update(float dt);
    void Draw();
    void Resize(Size size);
    void Start(object args);
    void Initialize(IIoCContainer container);
}