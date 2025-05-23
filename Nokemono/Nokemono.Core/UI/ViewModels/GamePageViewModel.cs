using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Nokemono.Core.Game;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Nokemono.Core.UI.ViewModels;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class GamePageViewModel: IPageCommandsSource, IDisposable
{
    private readonly IEventSource _eventSource;
    private readonly IRenderer2 _renderer;
    private readonly IKeyboard _keyboard;
    private readonly IAppHost _appHost;
    ICommand IPageCommandsSource.MenuCommand => _pauseCommand;
    ICommand IPageCommandsSource.BackCommand => null;
    
    private readonly SyncCommand _pauseCommand;
    
    public SharedStringValue Fps { get; } = new();
    public IGameInterfaces GameInterfaces { get; }
    public SharedBoolMutable ShowDebug { get; } = new(false);
    public SharedBoolMutable ShowFps { get; } = new(false);
    
    private double _fps = 60;

    private double _gameTime;
    private int _frames;

    private enum DebugMode
    {
        None = 0,
        Fps,
        Stats,
        Full
    }

    private DebugMode _debugMode = DebugMode.None;
    
    public GamePageViewModel(INavigation navigation, IEventSource eventSource, 
        IRenderer2 renderer, IGameInterfaces gameInterfaces, IKeyboard keyboard,
        IAppHost appHost)
    {
        _eventSource = eventSource;
        _renderer = renderer;
        _keyboard = keyboard;
        _appHost = appHost;
        _pauseCommand = new SyncCommand(() => navigation.NavigateTo<PausePageViewModel>(GameInterfaces));

        GameInterfaces = gameInterfaces;
        
        _eventSource.Updating += OnUpdating;
        _eventSource.RenderFinished += OnRenderFinished;
    }

    private void OnRenderFinished()
    {
    }

    private void OnUpdating(float dt)
    {
        if (_keyboard.GetKey(Key.D) == ButtonState.JustPressed)
        {
            _debugMode = (DebugMode)(((int)_debugMode + 1) % Enum.GetValues(typeof(DebugMode)).Length);

            if (_debugMode == DebugMode.None)
            {
                Fps.SetText("");
                ShowFps.SetValue(false);
            }
            else
            {
                ShowFps.SetValue(true);
            }

            if (_debugMode == DebugMode.Full)
            {
                ShowDebug.SetValue(true);
            }
            else
            {
                ShowDebug.SetValue(false);
            }
        }
        
        _gameTime += dt;
        _frames++;

        if (_gameTime > 1)
        {
            _fps = _frames / _gameTime;
            _frames = 0;
            _gameTime = 0;
        }

        switch (_debugMode)
        {
            case DebugMode.Fps:
                Fps.FormatText("FPS: {0}", (int)Math.Round(_fps));
                break;
            case DebugMode.Stats:
            case DebugMode.Full:
                Fps.FormatText("FPS: {0}\n" +
                               "Physics Target FPS: {1}\n" +
                               "Quads Rendered: {2}\n" +
                               "Sprites Rendered: {3}\n" +
                               "Lines Rendered: {4}\n" +
                               "Rectangles Filled: {5}\n" +
                               "Target Resolution: {6}x{7}\n" +
                                "Screen Resolution: {8}x{9}",
                    (int)Math.Round(_fps),
                    (int)Math.Round(1f / GameInterfaces.Instance.WorldDelta),
                    _renderer.QuadsRenderer.QuadsRendered,
                    _renderer.SpriteRenderer.SpritesRendered,
                    _renderer.GeometryRenderer.LinesRendered,
                    _renderer.GeometryRenderer.RectanglesFilled,
                    _appHost.TargetSize.Width, _appHost.TargetSize.Height,
                    _renderer.TargetSize.Width, _renderer.TargetSize.Height);
                break;
            
        }
    }

    public void Dispose()
    {
        GameInterfaces.Instance.Dispose();
        _eventSource.Updating -= OnUpdating;
        _eventSource.RenderFinished -= OnRenderFinished;
    }
}