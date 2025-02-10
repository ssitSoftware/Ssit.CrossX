// using System.Numerics;
// using Ssit.CrossX.Common;
// using Ssit.CrossX.Core;
// using Ssit.CrossX.Graphics;
// using Ssit.CrossX.IoC;
// using Ssit.CrossX.UI.Services;
//
// namespace Ssit.CrossX.UI;
//
// public abstract class UiPixelApp : PixelApp
// {
//     protected IUiApp UiApp { get; private set; }
//     private IAppHost _appHost;
//     private IRenderer _renderer;
//         
//     protected abstract RgbaColor BackgroundColor { get; }
//         
//     protected override void OnDispose(bool disposing)
//     {
//         base.OnDispose(disposing);
//
//         if (disposing)
//         {
//             UiApp?.Dispose();
//             UiApp = null;
//                 
//             _appHost?.Dispose();
//             _appHost = null;
//         }
//     }
//
//     protected override void OnInitialize(IIoCContainer container)
//     {
//         base.OnInitialize(container);
//             
//         _renderer = container.Get<IRenderingWindow>().Renderer;
//         _appHost = CreateAppHost(container);
//             
//         UiApp = container.InitializeUi(OnInitializeUi);
//         OnResize(_renderer.TargetSize);
//     }
//         
//     protected virtual void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
//     {
//         builder
//             .WithInstance<IInputCoordinateSystem>(new InputCoordinateSystem(_appHost))
//             .WithCommonUi();
//
//         handlers.AddCommonUiMaping();
//     }
//         
//     protected override void OnResize(Size size)
//     {
//         _appHost.Resize(size);
//         UiApp.SetBounds(new RectangleF(Vector2.Zero, _appHost.TargetSize / _appHost.Scale), _appHost.Scale);
//     }
//         
//     protected override void OnDraw()
//     {
//         _renderer.Clear(RgbaColor.Black);
//         _appHost.Render(Render);
//     }
//
//     private void Render(RenderMode mode)
//     {
//         UiApp.Draw(_renderer, mode, BackgroundColor);
//     }
//
//     public override void OnUpdate(float elapsedTime) => UiApp.Update(elapsedTime);
//
//     protected abstract IAppHost CreateAppHost(IIoCContainer container);
// }