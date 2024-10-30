using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Ssit.Pixel;
using Ssit.Pixel.Audio;
using Ssit.Pixel.Audio.Internal;
using Ssit.Pixel.Content;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Input;
using Ssit.Pixel.IO;
using Ssit.Pixel.IoC;
using Rectangle = Ssit.Pixel.Rectangle;
using Size = Ssit.Pixel.Size;

namespace SampleGame.Game;

public class GameApp: PixelApp
{
    private IKeyboard _keyboard;
    private IGameControllers _gameControllers;
    private IRenderer _renderer;
    private IContentManager _contentManager;
    private ISoundManager _soundManager;

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
        var bundleProviderType = builder.ImplementationMapper.ResolveImplementation<IFilesProvider>("Bundle");
        
        var bundleProvider = (IFilesProvider)Activator.CreateInstance(bundleProviderType);
        var embededProvider = new EmbededFilesProvider(typeof(GameApp).Assembly, "SampleGame.Assets");

        var filesProvider = new AggregatedFilesProvider();
        filesProvider.AddProvider("assets:", embededProvider);
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
        
        _musicPlayer.ChangePlaylist("Normal");

        var inputMappings = container.Get<IInputMappings>();
        var mapper = inputMappings.Mapper(0);

        mapper.MapAxis("Horizontal", GameControllerAxis.LeftX);
        mapper.MapAxis("Vertical", GameControllerAxis.LeftY);
        mapper.MapAxis("Horizontal", Key.Left, Key.Right);
        mapper.MapAxis("Vertical", Key.Up, Key.Down);
        
        mapper.MapButton("Fire", GameControllerButton.X);
        mapper.MapButton("Fire", GameControllerButton.Y);
        mapper.MapButton("Fire", GameControllerButton.A);
        mapper.MapButton("Fire", GameControllerButton.B);
        
        mapper.MapButton("Fire", Key.X);
        
        _player = container.IoCConstruct<Player>();
    }

    protected override void OnUpdate(float elapsedTime)
    {
        if (_keyboard.GetKey(Key.Down).IsDown)
        {
            _musicPlayer.Volume = MathF.Max(0, _musicPlayer.Volume - elapsedTime / 4);
        }
        
        if (_keyboard.GetKey(Key.Up).IsDown)
        {
            _musicPlayer.Volume = MathF.Min(1, _musicPlayer.Volume + elapsedTime / 4);
        }
        
        if (_keyboard.GetKey(Key.Left).IsDown)
        {
            _soundManager.MasterVolume = MathF.Max(0, _soundManager.MasterVolume - elapsedTime / 4);
        }
        
        if (_keyboard.GetKey(Key.Right).IsDown)
        {
            _soundManager.MasterVolume = MathF.Min(1, _soundManager.MasterVolume + elapsedTime / 4);
        }
        
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
    }

    protected override void OnResize(Size size)
    {
        _size = size;
    }
}