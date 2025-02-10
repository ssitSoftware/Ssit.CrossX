using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI;

internal interface IPage: IViewParent, IDisposable
{
    void Load(IUiServices services, IInputContext inputContext, object viewModel);
    void Update(float dt);
    void Draw(IRenderer2 renderer);
    void SetBounds(RectangleF bounds, float scale);
    ViewHandler RootHandler { get; }
    bool OnUiButton(UiButton button, IInputContext inputProcessor);
    IFocusable FocusedElement { get; set; }
    float TransitionTime { get; set; }
    float TransitionProgress { get; set; }
    float Scale { get; }
    void SignalRecalculationPending();
    void InvalidateRendering();
}