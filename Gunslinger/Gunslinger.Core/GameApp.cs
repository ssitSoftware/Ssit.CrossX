using System.Numerics;
using Gunslinger.Core.UI.Pages;
using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Common;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core
{
    public class GameApp : PixelApp, IInputCoordinateSystem, ISettingsProvider
    {
        private IRenderer _renderer;
        private IUiApp _uiApp;
        private PixelAppHost _appHost;

        public Settings Settings { get; private set; }
        
        public Matrix3x2 Transform
        {
            get
            {
                if (Matrix3x2.Invert(_appHost.Transform, out var matrix))
                {
                    return matrix;
                }

                return Matrix3x2.Identity;
            }
        }

        protected override void OnInitializeServices(IIoCContainerBuilder builder)
        {
            var bundleProvider = new BundleFilesProvider();
            var embeddedProvider = new EmbeddedFilesProvider(typeof(GameApp).Assembly, "Gunslinger.Core.Assets");

            var filesProvider = new AggregatedFilesProvider();
            filesProvider.AddProvider("assets:", embeddedProvider);
            filesProvider.AddProvider("bundle:", bundleProvider);
            
            builder
                .WithInstance<IFilesProvider>(filesProvider)
                .WithInstance<ISettingsProvider>(this);
        }

        private void OnInitializeUi(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper handlers)
        {
            builder
                .WithInstance<IInputCoordinateSystem>(this)
                .WithCommonUi();
            
            navigationMap
                .Map<MainPageViewModel, MainPage>()
                .Map<OptionsPageViewModel, OptionsPage>()
                .Map<GamePageViewModel, GamePage>();

            handlers.AddCommonUiMaping();
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

            var inputMappings = container.Get<IInputMappings>();
            var mapper = inputMappings.Mapper(0);

            mapper.MapAxis("Horizontal", GameControllerAxis.LeftX);
            mapper.MapAxis("Vertical", GameControllerAxis.LeftY);
            mapper.MapAxis("Horizontal", GameControllerButton.DPadLeft, GameControllerButton.DPadRight);
            mapper.MapAxis("Vertical", GameControllerButton.DPadUp, GameControllerButton.DPadDown);
            mapper.MapAxis("Horizontal", Key.A, Key.D);
            mapper.MapAxis("Vertical", Key.W, Key.S);

            mapper.MapAxis("AimX", GameControllerAxis.RightX);
            mapper.MapAxis("AimY", GameControllerAxis.RightY);

            mapper.MapButton("Shoot", GameControllerButton.RightShoulder);
            mapper.MapButton("Shoot", Key.MouseLeft);

            mapper.MapButton("Melee", GameControllerButton.X);
            mapper.MapButton("Melee", Key.MouseRight);

            mapper.MapButton("Roll", GameControllerButton.A);
            mapper.MapButton("Roll", Key.Space);

            mapper.MapButton("Reload", GameControllerButton.Y);
            mapper.MapButton("Reload", Key.R);

            var fontsManager = container.Get<IFontsManager>();
            fontsManager.LoadFonts("assets:/Fonts/Fonts.json");

            container.Get<IContentManager>().RegisterGameContentTypes();

            _uiApp = container.InitializeUi(OnInitializeUi);

            _appHost = container.IoCConstruct<PixelAppHost>(new PixelAppHost.Parameters
            {
                DesignSize = new Size(480, 360),
                Mode = PixelAppHost.Mode.Height,
                Renderer = _renderer,
                MaxScale = 2
            });
            
            Settings = Settings.Load(container.Get<IFileStorage>(), "Gunslinger/settings");
            
            container.Get<IMusicPlayer>().Volume = Settings.MusicVolume / 4f;
            container.Get<ISoundManager>().MasterVolume = Settings.SoundVolume / 4f;
            
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
            _renderer.Clear(RgbaColor.FromBgra(0xff513a3d));
            _uiApp.Draw(_renderer);
        }

        protected override void OnResize(Size size)
        {
            _appHost.Resize(size);
            _uiApp.SetBounds(new RectangleF(Vector2.Zero, _appHost.TargetSize / _appHost.Scale), _appHost.Scale);
        }
    }
}