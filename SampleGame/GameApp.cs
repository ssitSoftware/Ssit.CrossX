using System;
using System.Collections.Generic;
using System.Drawing;
using Ssit.Pixel;
using Ssit.Pixel.Content;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Input;
using Ssit.Pixel.IO;
using Ssit.Pixel.IoC;
using Rectangle = Ssit.Pixel.Rectangle;
using RectangleF = Ssit.Pixel.RectangleF;
using Size = Ssit.Pixel.Size;

namespace SampleGame;

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
    
    public GameApp()
    {
        for (var idx = 0; idx < 100; ++idx)
        {
            _entities.Add(new Entity());
        }
    }

    protected override void OnInitializeServices(IIoCContainerBuilder builder)
    {
        builder.WithInstance<IFilesProvider>(new EmbededFilesProvider(typeof(GameApp).Assembly));
    }

    protected override void OnInitialize(IIoCContainer container)
    {
        base.OnInitialize(container);
        
        _keyboard = container.Get<IKeyboard>();
        _gameControllers = container.Get<IGameControllers>();
        _renderer = container.Get<IRenderer>();
        _contentManager = container.Get<IContentManager>();

        _texture = _contentManager.Load<ITexture>("Assets/Image.jpg");
    }

    protected override void OnUpdate(float elapsedTime)
    {
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
        
        // foreach (var entity in _entities)
        // {
        //     _renderer.FillRectangle(new RectangleF(entity.Position.X - 50, entity.Position.Y - 50, 100, 100), entity.Color);
        // }
        
        _renderer.DrawTexture(_texture.Resource, 
            new Rectangle(0, 0, _size.Width, _size.Height));
    }

    protected override void OnResize(Size size)
    {
        _size = size;
    }
}