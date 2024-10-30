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

    private ResourceHandle<ITexture> _texture;

    private RgbaColor _backgroundColor = Color.IndianRed;

    private Size _size = Size.Zero;

    private readonly List<Entity> _entities = new();

    private float _cumulatedTime = 0;
    private const float TimeDelta = 1 / 120f; 
    
    private IRenderTarget _renderTarget;
    private IMusicPlayer _musicPlayer;
    
    private ResourceHandle<ISoundEffect> _soundEffect;
    
    public GameApp()
    {
        for (var idx = 0; idx < 100; ++idx)
        {
            _entities.Add(new Entity());
        }
    }

    protected override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        var bundleProviderType = builder.ImplementationMapper.ResolveImplementation<IFilesProvider>("Bundle");
        
        var bundleProvider = (IFilesProvider)Activator.CreateInstance(bundleProviderType);
        var embededProvider = new EmbededFilesProvider(typeof(GameApp).Assembly, "SampleGame");

        var filesProvider = new AggregatedFilesProvider();
        filesProvider.AddProvider("int:", embededProvider);
        filesProvider.AddProvider("bundle:", bundleProvider);
        
        builder.WithInstance<IFilesProvider>(filesProvider);
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
        
        container.Get<ISoundManager>().MasterVolume = 2;

        _texture = _contentManager.Get<ITexture>("int:/Assets/Image.jpg");
        _renderTarget = container.IoCConstruct<IRenderTarget>(new CreateRenderTargetParameters
        {
            Size = new Size(128, 128)
        });
        
        _soundEffect = _contentManager.Get<ISoundEffect>("int:/Assets/MenuSelect.wav");
        
        _musicPlayer.RegisterPlaylist("Normal", new MusicPlaylist
        {
            new Song("bundle:/Music/Desert.ogg")
        });
        
        _musicPlayer.RegisterPlaylist("Other", new MusicPlaylist
        {
            new Song("bundle:/Music/DriveInTunnel.ogg")
        });
        
        _musicPlayer.ChangePlaylist("Normal");
    }

    protected override void OnUpdate(float elapsedTime)
    {
        if (_keyboard.GetKey(Key.S) == ButtonState.JustPressed)
        {
            _soundEffect.Resource.PlayOnce(pitch: Random.Shared.NextSingle() * 1.51f + 0.5f);
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

        while (_cumulatedTime >= TimeDelta)
        {
            foreach (var entity in _entities)
            {
                entity.Update(TimeDelta, _size);
            }

            _cumulatedTime -= TimeDelta;
        }
    }

    protected override void OnDraw()
    {
        _renderer.Clear(_backgroundColor);
        
        _renderer.DrawTexture(_texture.Resource, 
            new Rectangle(10, 10, _texture.Resource.Size.Width * 4, _texture.Resource.Size.Height * 4), depth: 0);

        _renderer.SetTransform(Matrix3x2.CreateTranslation(_texture.Resource.Size.Width * 2, _texture.Resource.Size.Height * 2));
        
        _renderer.DrawTexture(_texture.Resource, 
            new Rectangle(10 , 10, _texture.Resource.Size.Width * 4, _texture.Resource.Size.Height * 4),
            depth: 100);
        
        _renderer.SetTransform(null);
        
        _renderer.SetRenderTarget(_renderTarget);
        
        _renderer.Clear(RgbaColor.Coral);
        _renderer.SetRenderTarget(null);
        
        _renderer.DrawTexture(_renderTarget, new Rectangle(0,0,128,128));
    }

    protected override void OnResize(Size size)
    {
        _size = size;
    }
}