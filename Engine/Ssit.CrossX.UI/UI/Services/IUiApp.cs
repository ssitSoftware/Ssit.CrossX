using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.UI.Services;

public interface IUiApp: IDisposable
{
    public IIoCContainer Services { get; }
    public INavigation Navigation { get; }

    void Update(float dt);
    void Draw(IRenderer renderer, RgbaColor? clearColor = null);
    void SetBounds(RectangleF bounds, float scale);
}