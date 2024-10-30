using System.Numerics;
using Ssit.Pixel;
using Ssit.Pixel.Graphics;
using Ssit.Pixel.Input;

namespace SampleGame.Game;

public class Player
{
    private readonly IInputMappings _inputMappings;

    private int _currentColor = 0;
    
    private RgbaColor[] _colors = [
        RgbaColor.Blue,
        RgbaColor.Red,
        RgbaColor.Green
    ];

    private Vector2 _position = new Vector2(200, 200);
    
    public Player(IInputMappings inputMappings)
    {
        _inputMappings = inputMappings;
    }

    public void Update()
    {
        if ( _inputMappings[0].GetButton("Fire") == ButtonState.JustPressed)
        {
            _currentColor++;
            _currentColor %= _colors.Length;
        }
    }
    
    public void Update(float dt)
    {
        var moveX = _inputMappings[0].GetAxis("Horizontal");
        var moveY = _inputMappings[0].GetAxis("Vertical");
        
        _position.X += moveX * dt * 250;
        _position.Y += moveY * dt * 250;
    }

    public void Draw(IRenderer renderer)
    {
        renderer.FillRectangle(new RectangleF(_position.X - 25, _position.Y - 25, 50, 50), _colors[_currentColor]);
    }
}