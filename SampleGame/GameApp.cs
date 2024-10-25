using System;
using System.Drawing;
using Ssit.Pixel;
using Ssit.Pixel.Core;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Input;
using Rectangle = Ssit.Pixel.Rectangle;

namespace SampleGame;

public class GameApp: PixelApp
{
    private readonly IKeyboard _keyboard;
    private readonly IGameControllers _gameControllers;
    private IRenderer _renderer;

    private RgbaColor _backgroundColor = Color.IndianRed;
    
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
    }

    protected override void OnDraw()
    {
        _renderer.Clear(_backgroundColor);
        _renderer.FillRectangle(new Rectangle(100,100,100,100), Color.Chartreuse);
    }
}