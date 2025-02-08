using System;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Games.Rendering.Map;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;
using Ssit.CrossX.IoC;

namespace Gunslinger.Core.Game;

public class Simulation : ISimulation
{
    private readonly IGameTemplate _gameTemplate;
    private readonly IInputMappings _inputMappings;
    private readonly MapDisplayElement _mapDisplayElement;
    
    int ISimulation.RenderPasses => 1;

    private Vector2 _position = new Vector2(0, 10000); 

    void ISimulation.Render(IRenderer renderer, RenderMode mode, RectangleF target, int renderPass, float scale)
    {
        if (renderPass != 0)
            return;
        
        Render(renderer, target, mode, scale);
    }

    public Simulation(IIoCContainer container, IContentManager contentManager, IGameTemplate gameTemplate, IInputMappings inputMappings, string path)
    {
        _gameTemplate = gameTemplate;
        _inputMappings = inputMappings;

        using var stream = contentManager.FilesProvider.Open(path);
        var map = MapFile.FromStream(stream, gameTemplate);

        _position.Y = map.MainLayer.Size.Height - (float)gameTemplate.TargetSize.Height / gameTemplate.TileSize;
        _position.X = (float)gameTemplate.TargetSize.Width / gameTemplate.TileSize / 2;
        
        var builder = new MapDisplayElementBuilder()
            .WithServices(container, contentManager)
            .WithTemplate(gameTemplate)
            .WithMap(map);

        _mapDisplayElement = builder.Build();
    }
    
    public void RenderDebug(IRenderer renderer, RectangleF target, float scale)
    {
    }

    void ISimulation.Update(float deltaTime) => Update(deltaTime);

    public event Action Updated;

    private void Render(IRenderer renderer, RectangleF target, RenderMode renderMode, float scale)
    {
        renderer.StateManager.SaveState();

        if (renderMode == RenderMode.Normal)
        {
            renderer.FillRectangle(target, _gameTemplate.DefaultBackground);
        }
        else
        {
            renderer.FillRectangle(target, RgbaColor.Black);
        }
        
        renderer.StateManager.Transform(Matrix3x2.CreateTranslation(target.TopLeft));
        renderer.StateManager.Transform(Matrix3x2.CreateScale(scale));

        var size = target.Size.ToVector() / scale;
        
        MapRenderer.Render(renderer, _mapDisplayElement, null, _position,
            new Size((int)MathF.Ceiling(size.X), (int)MathF.Ceiling(size.Y)),
            _gameTemplate.TileSize, renderMode);

        renderer.StateManager.RestoreState();
    }

    private void Update(float deltaTime)
    {
        _position.X += _inputMappings[0].GetAxis("Horizontal") * deltaTime * 10;
        _position.Y += _inputMappings[0].GetAxis("Vertical") * deltaTime * 10;
        
        _mapDisplayElement.Update(deltaTime);
        Updated?.Invoke();
    }
    
    public void Dispose()
    {
        _mapDisplayElement.Dispose();
    }
}