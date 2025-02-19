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
        public Action<WorldBuilder> InitWorldFunc { get; set; }
    }
    
    private const float WorldDelta = 1f / 60f;
    
    private readonly IActionScheduler _scheduler;
    private readonly IGameTemplate _gameTemplate;
    private readonly IInputMappings _inputMappings;
    private readonly MapDisplayElement _mapDisplayElement;

    public readonly World World;
    private readonly IIoCContainer _container;
    private readonly ICamera _camera;
    
    private bool _isDisposed;

    private float _timeToUpdate = 0;
    
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
        _inputMappings = inputMappings;

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

        parameters.InitWorldFunc?.Invoke(worldBuilder);
        
        (World, _container) = worldBuilder.Build();

        _camera = _container.Get<ICamera>();
    }

    void IGameInstance.Update(float deltaTime) => Update(deltaTime);

    public event Action Updated;

    private void Render(IRenderer2 renderer, RectangleF target, float scale)
    {
        if (_isDisposed)
            return;
        
        renderer.StateManager.SaveState();
        renderer.GeometryRenderer.FillRectangle(target, renderer.StateProvider.UseGlowTextures ? RgbaColor.Black : _gameTemplate.DefaultBackground);
        
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
            if (body.UserData is IUpdatable updatable)
            {
                updatable.Update(deltaTime);
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
                if (body.UserData is IUpdatable updatable)
                {
                    updatable.FixedUpdate(worldDelta);
                }
            }
            
            World.Step(worldDelta);
            _timeToUpdate -= worldDelta;
            
            foreach (var body in World.BodyList)
            {
                if (body.UserData is IUpdatable updatable)
                {
                    updatable.PostFixedUpdate();
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
            if (body.UserData is IUpdatable updatable)
            {
                updatable.PostUpdate();
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
        Task.Delay(1000).ContinueWith(o =>
        {
            _scheduler.Schedule(_mapDisplayElement.Dispose);
            foreach (var body in World.BodyList)
            {
                World.RemoveBody(body);
            }

            World.ProcessChanges();
            _container?.Dispose();
        });
    }
}