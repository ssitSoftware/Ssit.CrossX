using System.Drawing;
using Ssit.Pixel.Framework.Core;
using Ssit.Pixel.Framework.Graphics;
using Ssit.Pixel.Framework.Input;
using Rectangle = Ssit.Pixel.Framework.Rectangle;

namespace SampleGame;

public class GameApp: PixelApp
{
    private readonly IKeyboard _keyboard;
    private readonly IGameControllers _gameControllers;
    private IRenderer _renderer;
    
    public GameApp(IRenderingDevice device, IKeyboard keyboard, IGameControllers gameControllers, WindowParameters windowParameters) : base(windowParameters)
    {
        _keyboard = keyboard;
        _gameControllers = gameControllers;
        _renderer = device.Renderer;
    }

    protected override void OnDraw()
    {
        _renderer.Clear(Color.IndianRed);
        _renderer.FillRectangle(new Rectangle(100,100,100,100), Color.Chartreuse);
    }
}