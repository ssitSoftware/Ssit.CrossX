using System.Numerics;
using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core
{
    public class GameApp : PixelApp
    {
        private IRenderer _renderer;
        private IUiApp _uiApp;
        private PixelAppHost _appHost;
        private readonly IGameTemplate _template = new GunslingerTemplate();

        protected override void OnInitializeServices(IIoCContainerBuilder builder)
        {
            var filesProvider = _template.AssetsProvider;

            builder
                .WithInstance(filesProvider)
                .WithSingleton<ISettingsProvider, SettingsProvider>()
                .WithInstance(_template);
        }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);
            _uiApp.Dispose();
        }

        protected override void OnInitialize(IIoCContainer container)
        {
            base.OnInitialize(container);

            _renderer = container.Get<IRenderingWindow>().Renderer;

            container
                .InitializeInputMapping()
                .InitializeFonts()
                .InitializeGame()
                .InitializeMusic("Menu");

            var appHostParameters = new PixelAppHost.Parameters
            {
                Renderer = _renderer,
                DesignSize = _template.TargetSize,
                Mode = PixelAppHost.Mode.Height,
                MaxScale = 4
            }; 
            
            _appHost = container.IoCConstruct<PixelAppHost>(appHostParameters);
            _uiApp = container.InitializeUi(new InputCoordinateSystem(_appHost));
            
            OnResize(_renderer.TargetSize);
            _uiApp.Navigation.NavigateTo<MainPageViewModel>();
        }
        
        public override void OnUpdate(float elapsedTime) => _uiApp.Update(elapsedTime);

        protected override void OnDraw()
        {
            _renderer.Clear(RgbaColor.Black);
            _appHost.Render(Render);
        }

        private void Render()
        {
            _renderer.Clear(RgbaColor.FromBgra(0xff101010));
            _uiApp.Draw(_renderer);
        }

        protected override void OnResize(Size size)
        {
            _appHost.Resize(size);
            _uiApp.SetBounds(new RectangleF(Vector2.Zero, _appHost.TargetSize / _appHost.Scale), _appHost.Scale);
        }
    }
}