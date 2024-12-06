using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Text;
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
using RectangleF = Ssit.CrossX.RectangleF;
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
    private IIoCContainer _iocContainer;

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

    private float _fps = 0;
    
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

        _iocContainer = container;
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
        
        PrepareText();
    }

    protected override void OnUpdate(float elapsedTime)
    {
        _fps = (_fps + 1 / elapsedTime) / 2;
        
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

    private readonly TextRenderingContext _renderingContext = new();
    private readonly StringBuilder _fpsText = new ();

    private IVertexBuffer[] _textVertices;

    private void PrepareText()
    {
        if (_textVertices is not null)
        {
            foreach (var buffer in _textVertices)
            {
                buffer.Dispose();
            }
        }
        
        var font = (IGlyphFont)_fontsManager.GetFont("Default", 32);

        var text =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed dapibus, diam sed dignissim dapibus, mi arcu fringilla elit, ac efficitur nibh elit ut velit. Donec finibus libero vel hendrerit pulvinar. Vivamus laoreet lectus tortor, rhoncus aliquet ipsum convallis vitae. Integer ut tempor nunc, in malesuada nulla. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Integer sagittis justo sit amet aliquam tincidunt. Integer vitae fringilla felis. Fusce lobortis erat fringilla, ornare ante eu, pellentesque tellus.\n" +
            "Vestibulum nec urna ut felis luctus viverra. Praesent varius velit eget metus lacinia elementum. Praesent nec enim vel neque sollicitudin consectetur. Sed tristique enim vitae quam ullamcorper accumsan eleifend quis sapien. Sed varius odio quis pharetra fermentum. Proin mauris elit, ullamcorper vitae aliquet ac, dignissim vitae risus. Vestibulum fringilla blandit tellus, sed maximus orci mollis quis.\n" +
            "Cras eros nibh, pulvinar ut tellus sed, viverra euismod lacus. Nullam nec magna pharetra, lacinia arcu ac, facilisis tortor. Praesent nec urna et arcu suscipit placerat quis a ante. Maecenas tincidunt nunc mi, ac imperdiet ipsum blandit quis. Duis molestie finibus tincidunt. Ut ultricies quam libero, ultricies semper elit lobortis placerat. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque sagittis sapien orci, sed lobortis diam malesuada vel. Aenean sit amet elit varius, sagittis leo at, fringilla ante. Donec bibendum condimentum aliquam.";

        _textVertices = _iocContainer.CreateMultilineTextPrimitives(font, text, new RectangleF(100, 100, 500, 1000), TextAlign.Justified, TextSpacing.Normal, font.LineSize / 4f);
    }
    
    protected override void OnDraw()
    {
        _renderer.Clear(_backgroundColor);
        _player.Draw(_renderer);
        
        var font = (IGlyphFont)_fontsManager.GetFont("Default", 32);

        foreach (var buffer in _textVertices)
        {
            _renderer.DrawPrimitives(buffer, 0, buffer.Length, font.OutlineSheet, RgbaColor.Black); 
        }

        foreach (var buffer in _textVertices)
        {
            _renderer.DrawPrimitives(buffer, 0, buffer.Length, font.FontSheet, RgbaColor.White); 
        }
        
        _fpsText.Clear();
        _fpsText.AppendFormat("FPS: {0}", (int)MathF.Ceiling(_fps));
        
        _renderer.DrawText(font, _fpsText, new Vector2(10, 40));
    }

    protected override void OnResize(Size size)
    {
        _size = size;
    }
}