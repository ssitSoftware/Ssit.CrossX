using System;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.XxFormats.Map;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering.Map;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.XxFormats.Template;
using Ssit.IoC;

namespace Ssit.CrossX.Games.Logic;

public class GameInstance : IGameInstance
{
    public class Parameters
    {
        public string MapPath { get; set; }
        public Action<World> ProcessWorldFunc { get; set; }
        public Action<IIoCContainerBuilder> RegisterServices { get; set; }
    }
    
    public event Action<float> FixedUpdate;
    public float WorldDelta => _timer.TimeDelta;
    private readonly GameTimer _timer = new();

    private readonly IActionScheduler _scheduler;
    private readonly IGameTemplate _gameTemplate;
    private readonly IPaletteSource _paletteSource;
    private readonly MapDisplayElement _mapDisplayElement;

    // ReSharper disable once MemberCanBePrivate.Global
    public readonly World World;
    public readonly IIoCContainer Container;
    private readonly ICamera _camera;
    
    private bool _isDisposed;
    private float _timeToUpdate;

    private int _bgColorIndex = 0;
    
    int IGameInstance.RenderPasses => 1;
    
    void IGameInstance.Render(IRenderer2 renderer, RectangleF target, int renderPass, float scale)
    {
        if (renderPass != 0)
            return;
        
        Render(renderer, target, scale);
    }

    void IGameInstance.RenderDebug(IRenderer2 renderer, RectangleF target, float scale) => RenderDebug(renderer, target, scale);

    public GameInstance(IIoCContainer container, IContentManager contentManager,
        IActionScheduler scheduler, IGameTemplate gameTemplate,
        Parameters parameters, IPaletteSource paletteSource = null)
    {
        _scheduler = scheduler;
        _gameTemplate = gameTemplate;
        _paletteSource = paletteSource;

        using var stream = contentManager.FilesProvider.Open(parameters.MapPath);
        var map = MapFile.FromStream(stream, gameTemplate);

        var tilesets = map.Tilesets.Select(contentManager.Get<ITexture>).ToArray();
        
        var builder = new MapDisplayElementBuilder()
            .WithServices(container, contentManager)
            .WithTemplate(gameTemplate)
            .WithMap(map);
        
        _mapDisplayElement = builder.Build();
        
        foreach (var ts in tilesets)
        {
            ts.Dispose();
        }

        var worldBuilder = new WorldBuilder()
            .WithMap(map)
            .WithFilesProvider(contentManager.FilesProvider)
            .WithContainer(container)
            .WithServicesRegistrar( b =>
            {
                parameters.RegisterServices(b);
                b.WithInstance<IGameInstance>(this);
            })
            .WithGameTemplate(gameTemplate);
        
        (World, Container) = worldBuilder.Build();
        parameters.ProcessWorldFunc?.Invoke(World);
        _camera = Container.Get<ICamera>();
    }

    void IGameInstance.Update(float deltaTime) => Update(deltaTime);

    public event Action Updated;

    protected virtual void Render(IRenderer2 renderer, RectangleF target, float scale)
    {
        if (_isDisposed)
            return;

        var bgColor = GetBgColor();

        renderer.StateManager.SaveState();
        renderer.GeometryRenderer.FillRectangle(target, renderer.StateProvider.UseGlowTextures ? RgbaColor.Black : bgColor);
        
        renderer.StateManager.Translate(target.TopLeft);
        renderer.StateManager.Scale(scale);

        var size = target.Size.ToVector() / scale;
        
        MapRenderer.Render(renderer, _mapDisplayElement, World, _camera.LookAt,
            new Size((int)MathF.Ceiling(size.X), (int)MathF.Ceiling(size.Y)),
            _gameTemplate.TileSize);

        renderer.StateManager.RestoreState();
    }

    protected RgbaColor GetBgColor()
    {
        var bgColor = _mapDisplayElement.BackgroundColor;
        if (_paletteSource is not null)
        {
            if (_bgColorIndex == 0)
            {
                float dist = float.MaxValue;
                for (var idx = 1; idx < _paletteSource.OriginalPalette.Count; ++idx)
                {
                    var srcColor = _paletteSource.OriginalPalette[idx];
                    var d = srcColor.DistanceTo(bgColor);

                    if (d < dist)
                    {
                        dist = d;
                        _bgColorIndex = idx;
                    }
                }
            }
            
            return _paletteSource.Palette[_bgColorIndex];
        }

        return bgColor;
    }

    private void RenderDebug(IRenderer2 renderer, RectangleF target, float scale)
    {
        if (_isDisposed)
            return;

        if (renderer.StateProvider.UseGlowTextures)
            return;
        
        renderer.StateManager.SaveState();
        renderer.StateManager.Translate(target.TopLeft);
        renderer.StateManager.Scale(scale);
        
        var size = target.Size.ToVector() / scale;
        MapRenderer.RenderDebug(renderer, _mapDisplayElement, World, _camera.LookAt,
            new Size((int)MathF.Ceiling(size.X), (int)MathF.Ceiling(size.Y)),
            _gameTemplate);
        
        renderer.StateManager.RestoreState();
    }

    private void Update(float deltaTime)
    {
        if (_isDisposed)
            return;

        _timer.Update(deltaTime);

        foreach (var body in World.BodyList)
        {
            if (body.Owner is IUpdatable updatable2)
            {
                updatable2.Update(deltaTime);
            }
        }

        float dt;
        while ( (dt = _timer.FixedTimeToUpdate()) > 0)
        {
            foreach (var body in World.BodyList)
            {
                if (body.Owner is IUpdatable updatable2)
                {
                    updatable2.FixedUpdate(dt);
                }
            }
            
            FixedUpdate?.Invoke(dt);
            World.Step(dt);
            
            foreach (var body in World.BodyList)
            {
                if (body.Owner is IUpdatable updatable2)
                {
                    updatable2.PostFixedUpdate();
                }
            }
            
            _camera.Update(dt);
        }
        
        foreach (var body in World.BodyList)
        {
            if (body.Owner is IUpdatable updatable2)
            {
                updatable2.PostUpdate();
            }
        }
        
        _mapDisplayElement.Update(deltaTime);
        Updated?.Invoke();
    }
    
    public void Dispose()
    {
        if (_isDisposed)
            return;
        
        _isDisposed = true;
        Task.Delay(1000).ContinueWith(_ =>
        {
            _scheduler.Schedule(_mapDisplayElement.Dispose);
            World.Dispose();
            Container?.Dispose();
        });
    }
}