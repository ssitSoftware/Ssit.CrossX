using System;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.IoC;

namespace Ssit.CrossX.UI.Services;

public interface IUiApp: IDisposable
{
    public IIoCContainer Services { get; }
    public INavigation Navigation { get; }

    void Update(float dt);
    void Draw(IRenderer2 renderer, RgbaColor? clearColor = null);
    void SetBounds(RectangleF bounds, float scale);
    void LoadStyles(params Type[] types);
}