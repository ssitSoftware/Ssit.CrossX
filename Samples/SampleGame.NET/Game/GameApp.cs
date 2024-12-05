using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;
using Rectangle = Ssit.CrossX.Rectangle;
using Size = Ssit.CrossX.Size;

namespace SampleGame.Game;

public class GameApp: PixelApp
{
    private IKeyboard _keyboard;
    private IGameControllers _gameControllers;
    private IRenderer _renderer;
    private IContentManager _contentManager;
    private ISoundManager _soundManager;
    private IFontsManager _fontsManager;

    private ResourceHandle<ITexture> _texture;

    private RgbaColor _backgroundColor = Color.IndianRed;

    private Size _size = Size.Zero;

    private Player _player;

    private float _cumulatedTime = 0;
    private const float TimeDelta = 1 / 120f; 
    
    private IRenderTarget _renderTarget;
    private IMusicPlayer _musicPlayer;

    private ISoundEffectInstance _seInstance;
    
    private ResourceHandle<ISoundEffect> _soundEffect;

    protected override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        var bundleProvider = new BundleFilesProvider();
        var embeddedProvider = new EmbeddedFilesProvider(typeof(GameApp).Assembly, "SampleGame.Assets");

        var filesProvider = new AggregatedFilesProvider();
        filesProvider.AddProvider("assets:", embeddedProvider);
        filesProvider.AddProvider("bundle:", bundleProvider);

        builder
            .WithInstance<IFilesProvider>(filesProvider);
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);

        if (disposing)
        {
            _soundEffect?.Dispose();
            _soundEffect = null;
            
            _renderTarget?.Dispose();
            _renderer = null;
            
            _texture?.Dispose();
            _texture = null;
        }
    }

    protected override void OnInitialize(IIoCContainer container)
    {
        base.OnInitialize(container);
        
        _keyboard = container.Get<IKeyboard>();
        _gameControllers = container.Get<IGameControllers>();
        _renderer = container.Get<IRenderingWindow>().Renderer;
        _contentManager = container.Get<IContentManager>();
        _musicPlayer = container.Get<IMusicPlayer>();
        _soundManager = container.Get<ISoundManager>();

        _texture = _contentManager.Get<ITexture>("assets:/Image.jpg");
        _renderTarget = container.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
        {
            Size = new Size(128, 128)
        });
        
        _soundEffect = _contentManager.Get<ISoundEffect>("assets:/MenuSelect.wav");
        _seInstance = _soundEffect.Resource.CreateInstance();
        
        _seInstance.Parameters = new SoundParameters
        {
            Volume = 1,
            Pitch = 1
        };
        
        _musicPlayer.RegisterPlaylist("Normal", new MusicPlaylist
        {
            new Song("bundle:/Music/Desert.ogg")
        });
        
        _musicPlayer.RegisterPlaylist("Other", new MusicPlaylist
        {
            new Song("bundle:/Music/DriveInTunnel.ogg")
        });
        
        //_musicPlayer.ChangePlaylist("Normal");

        var inputMappings = container.Get<IInputMappings>();
        var mapper = inputMappings.Mapper(0);

        mapper.MapAxis("Horizontal", GameControllerAxis.LeftX);
        mapper.MapAxis("Vertical", GameControllerAxis.LeftY);
        mapper.MapAxis("Horizontal", GameControllerButton.DPadLeft, GameControllerButton.DPadRight);
        mapper.MapAxis("Vertical", GameControllerButton.DPadUp, GameControllerButton.DPadDown);
        mapper.MapAxis("Horizontal", Key.Left, Key.Right);
        mapper.MapAxis("Vertical", Key.Up, Key.Down);
        
        mapper.MapButton("Fire", GameControllerButton.X);
        mapper.MapButton("Fire", Key.X);
        
        _fontsManager = container.Get<IFontsManager>();
        _fontsManager.LoadFonts("assets:/Fonts/Fonts.json");
        
        _player = container.IoCConstruct<Player>();
    }

    protected override void OnUpdate(float elapsedTime)
    {
        if (_keyboard.GetKey(Key.S) == ButtonState.JustPressed)
        {
            if (_seInstance.IsPlaying)
            {
                _seInstance.Stop();
            }
            else
            {
                _seInstance.Play(true);
            }
        }
        
        if (_keyboard.GetKey(Key.T) == ButtonState.JustPressed)
        {
            _musicPlayer.ChangePlaylist("Other", 0);
        }
        
        if (_keyboard.GetKey(Key.Y) == ButtonState.JustPressed)
        {
            _musicPlayer.ChangePlaylist("Normal", 0);
        }
        
        _backgroundColor = RgbaColor.GreenYellow;

        _cumulatedTime += elapsedTime;

        _player.Update();
        while (_cumulatedTime >= TimeDelta)
        {
            _player.Update(TimeDelta);
            _cumulatedTime -= TimeDelta;
        }
    }

    protected override void OnDraw()
    {
        _renderer.Clear(_backgroundColor);
        _player.Draw(_renderer);

        var font = _fontsManager.GetFont("Default", 32);
        
        
        
        _renderer.DrawText(font, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus sagittis sed odio et euismod.\n" +
                                 "Cras quis sem pharetra, sagittis duia, tincidunt sem. Donec vel odio nec diam varius rhoncus.\n" +
                                 "Duis facilisis magna vel imperdiet ultricies. Mauris finibus elit ut mauris egestas,\n" +
                                 "vitae rutrum mauris mattis. Mauris feugiat, mauris quis luctus lacinia,\n" +
                                 "nunc metus luctus sem, non ornare mi turpis non odio. Mauris nec eleifend urna, eget scelerisque nibh.\n" +
                                 "Cras et metus magna. Suspendisse sollicitudin velit id sodales tristique.", new Vector2(20, 100), RgbaColor.Yellow, outlineColor: RgbaColor.Brown);
        
        
    }

    protected override void OnResize(Size size)
    {
        _size = size;
    }
}