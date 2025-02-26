using System;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering.Map;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.Games.Logic;

public class GameInstance : IGameInstance
{
    public class Parameters
    {
        public string MapPath { get; set; }
        public Action<World> ProcessWorldFunc { get; set; }
    }
    
    private const float WorldDelta = 1f / 60f;
    
    private readonly IActionScheduler _scheduler;
    private readonly IGameTemplate _gameTemplate;
    private readonly MapDisplayElement _mapDisplayElement;

    // ReSharper disable once MemberCanBePrivate.Global
    public readonly World World;
    public readonly IIoCContainer Container;
    private readonly ICamera _camera;
    
    private bool _isDisposed;

    private float _timeToUpdate;
    
    int IGameInstance.RenderPasses => 1;
    
    void IGameInstance.Render(IRenderer2 renderer, RectangleF target, int renderPass, float scale)
    {
        if (renderPass != 0)
            return;
        
        Render(renderer, target, scale);
    }

    void IGameInstance.RenderDebug(IRenderer2 renderer, RectangleF target, float scale) => RenderDebug(renderer, target, scale);

    public GameInstance(IIoCContainer container, IContentManager contentManager, 
        IActionScheduler scheduler, IGameTemplate gameTemplate, IInputMappings inputMappings, 
        Parameters parameters)
    {
        _scheduler = scheduler;
        _gameTemplate = gameTemplate;

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
            .WithGameTemplate(gameTemplate);
        
        (World, Container) = worldBuilder.Build();
        parameters.ProcessWorldFunc?.Invoke(World);
        _camera = Container.Get<ICamera>();
    }

    void IGameInstance.Update(float deltaTime) => Update(deltaTime);

    public event Action Updated;

    private void Render(IRenderer2 renderer, RectangleF target, float scale)
    {
        if (_isDisposed)
            return;
        
        renderer.StateManager.SaveState();
        renderer.GeometryRenderer.FillRectangle(target, renderer.StateProvider.UseGlowTextures ? RgbaColor.Black : _mapDisplayElement.BackgroundColor);
        
        renderer.StateManager.Translate(target.TopLeft);
        renderer.StateManager.Scale(scale);

        var size = target.Size.ToVector() / scale;
        
        MapRenderer.Render(renderer, _mapDisplayElement, World, _camera.LookAt,
            new Size((int)MathF.Ceiling(size.X), (int)MathF.Ceiling(size.Y)),
            _gameTemplate.TileSize);

        renderer.StateManager.RestoreState();
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

        _timeToUpdate += deltaTime;

        foreach (var body in World.BodyList)
        {
            if (body.Owner is IUpdatable updatable2)
            {
                updatable2.Update(deltaTime);
            }
        }

        var worldDelta = WorldDelta;
        if (MathF.Abs(_timeToUpdate - worldDelta) < WorldDelta / 4f)
        {
            worldDelta = _timeToUpdate;
        }
        
        while (_timeToUpdate >= worldDelta)
        {
            foreach (var body in World.BodyList)
            {
                if (body.Owner is IUpdatable updatable2)
                {
                    updatable2.FixedUpdate(worldDelta);
                }
            }
            
            World.Step(worldDelta);
            _timeToUpdate -= worldDelta;
            
            foreach (var body in World.BodyList)
            {
                if (body.Owner is IUpdatable updatable2)
                {
                    updatable2.PostFixedUpdate();
                }
            }
            
            _camera.Update(worldDelta);
            
            worldDelta = WorldDelta;
            if (MathF.Abs(_timeToUpdate - worldDelta) < WorldDelta / 4f)
            {
                worldDelta = _timeToUpdate;
            }
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