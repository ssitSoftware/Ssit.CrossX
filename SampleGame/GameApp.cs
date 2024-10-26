using System;
using System.Drawing;
using System.Numerics;
using Ssit.Pixel;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Input;
using RectangleF = Ssit.Pixel.RectangleF;
using Size = Ssit.Pixel.Size;

namespace SampleGame;

public class GameApp: PixelApp
{
    private readonly IKeyboard _keyboard;
    private readonly IGameControllers _gameControllers;
    private IRenderer _renderer;

    private RgbaColor _backgroundColor = Color.IndianRed;

    private Size _size = Size.Zero;

    private Vector2 _position = new(50, 50);
    private Vector2 _direction = new(1, 1);
    
    public GameApp(IRenderingDevice device, IKeyboard keyboard, IGameControllers gameControllers, WindowParameters windowParameters) : base(windowParameters)
    {
        _keyboard = keyboard;
        _gameControllers = gameControllers;
        _renderer = device.Renderer;
    }
    
    private Random _random = new Random();
    
    protected override void OnUpdate(float elapsedTime)
    {
        if (_keyboard.GetKey(Key.W) == ButtonState.JustPressed)
        {
            _backgroundColor = RgbaColor.FromInt32( (int)(_random.Next() | 0xff000000));
        }
        
        if (_keyboard.GetKey(Key.T) == ButtonState.JustPressed)
        {
            WindowParameters.Width = 400;
            WindowParameters.Height = 300;
            WindowParameters.Apply();
        }

        if (_gameControllers.GetButton(0, GameControllerButton.A) == ButtonState.JustPressed)
        {
            _backgroundColor = Color.Indigo;
        }

        _position += _direction * elapsedTime * 500;

        var maxX = _size.Width - 50;
        var maxY = _size.Height - 50;
        
        var minX = 50;
        var minY = 50;
        
        if ( _position.X < minX )
        {
            _position.X = minY + (minY - _position.X);
            _direction.X = 1;
        }
        
        if ( _position.Y < minY )
        {
            _position.Y = minY + (minY - _position.Y);
            _direction.Y = 1;
        }
        
        if ( _position.X > maxX )
        {
            _position.X = maxX - (_position.X - maxX);
            _direction.X = -1;
        }
        
        if ( _position.Y > maxY )
        {
            _position.Y = maxY - (_position.Y - maxY);
            _direction.Y = -1;
        }
    }

    protected override void OnDraw()
    {
        _renderer.Clear(_backgroundColor);
        _renderer.FillRectangle(new RectangleF(_position.X - 50, _position.Y - 50, 100, 100), Color.Chartreuse);
    }

    protected override void OnResize(Size size)
    {
        _size = size;
    }
}