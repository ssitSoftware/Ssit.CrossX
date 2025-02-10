using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.IO;
using Ssit.CrossX.IoC;

namespace SDLTestApp;

class TestApp : IApp
{
    void IApp.SetActive(bool active) => IsActive = active;
    
    void IDisposable.Dispose()
    {
    }

    private Vector2 _position = new Vector2(100, 100); 
    
    public bool IsActive { get; private set; }
    
    private IKeyboard _keyboard;
    
    public void InitializeServices(IIoCContainerBuilder builder)
    {
        builder.WithInstance<IFilesProvider>(new EmbeddedFilesProvider(GetType().Assembly, "SDLTextApp.Assets"));
    }

    public void Update(float dt)
    {
        if (_keyboard.GetKey(Key.Left).IsDown)
        {
            _position.X -= 200 * dt;
        }
        
        if (_keyboard.GetKey(Key.Right).IsDown)
        {
            _position.X += 200 * dt;
        }
            
        if (_keyboard.GetKey(Key.Up).IsDown)
        {
            _position.Y -= 200 * dt;
        }
        
        if (_keyboard.GetKey(Key.Down).IsDown)
        {
            _position.Y += 200 * dt;
        }
    }

    public void Draw(IRenderer2 renderer)
    {
        renderer.Clear(RgbaColor.Yellow);
        renderer.GeometryRenderer.FillRectangle( new RectangleF(_position.X, _position.Y, 100, 100), RgbaColor.Red);
    }

    public void Resize(Size size)
    {
    }

    public void Start(object args)
    {
    }

    public void Initialize(IIoCContainer container)
    {
        _keyboard = container.Get<IKeyboard>();
        var windowManager = container.Get<IAppWindowManager>();
        
        windowManager.SetTitle("Test Application");
        windowManager.SetWindowed(new Size(1280, 720));
    }
}