using System.ComponentModel;
using System.Numerics;
using Gunslinger.Core.UI.Pages;
using Gunslinger.Core.UI.ViewModels;
using Ssit.CrossX;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Common;
using Ssit.CrossX.Common.Pages;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Template;
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
        private readonly IGameTemplate _template = new GunslingerTemplate();
        private PixelAppHost.Parameters _appHostParameters;
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
            var filesProvider = _template.AssetsProvider;

            builder
                .WithInstance(filesProvider)
                .WithInstance<ISettingsProvider>(this)
                .WithInstance(_template);
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
            mapper.MapAxis("Horizontal", GameControllerButton.DPadLeft, GameControllerButton.DPadRight);
            mapper.MapAxis("Horizontal", Key.Left, Key.Right);

            mapper.MapButton("Shoot", GameControllerButton.B);
            mapper.MapButton("Shoot", Key.Z);

            mapper.MapButton("Melee", GameControllerButton.X);
            mapper.MapButton("Melee", Key.X);

            mapper.MapButton("Jump", GameControllerButton.A);
            mapper.MapButton("Jump", Key.C);

            var fontsManager = container.Get<IFontsManager>();
            fontsManager.LoadFonts("assets:/Fonts/Fonts.json");

            container.Get<IContentManager>().RegisterGameContentTypes();
            
            _uiApp = container.InitializeUi(OnInitializeUi);
            _uiApp.Services.Get<PageInputContext>().AlwaysShowFocus = true;
            _uiApp.Services.Get<IPointingDevices>().Enable = false;
            
            Settings = Settings.Load(container.Get<IFileStorage>(), "Gunslinger/settings");
            Settings.PropertyChanged += UpdateSettings;
            
            UpdateSettings(this, new PropertyChangedEventArgs(nameof(Settings.MusicVolume)));
            UpdateSettings(this, new PropertyChangedEventArgs(nameof(Settings.SoundVolume)));
            
            _appHostParameters = new PixelAppHost.Parameters
            {
                Renderer = _renderer,
                DesignSize = _template.TargetSize,
                Mode = PixelAppHost.Mode.Height,
                MaxScale = 4
            };
            
            _appHost = container.IoCConstruct<PixelAppHost>(_appHostParameters);
            
            OnResize(_renderer.TargetSize);
            _uiApp.Navigation.NavigateTo<MainPageViewModel>();
        }

        private void UpdateSettings(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(Settings.MusicVolume):
                    _uiApp.Services.Get<IMusicPlayer>().Volume = Settings.MusicVolume / 4f;
                    break;
                
                case nameof(Settings.SoundVolume):
                    _uiApp.Services.Get<ISoundManager>().MasterVolume = Settings.SoundVolume / 4f;
                    break;
            }
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