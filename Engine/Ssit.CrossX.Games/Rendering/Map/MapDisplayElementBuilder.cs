using System.Collections.Generic;
using Ssit.CrossX.Content;
using Ssit.CrossX.XxFormats.Map;
using Ssit.CrossX.XxFormats.Template;
using Ssit.IoC;

namespace Ssit.CrossX.Games.Rendering.Map;

public class MapDisplayElementBuilder
{
    private MapFile _file;
    
    private IIoCContainer _container;
    private IContentManager _contentManager;
    private IGameTemplate _gameTemplate;
    
    public MapDisplayElementBuilder WithServices(IIoCContainer container, IContentManager contentManager)
    {
        _container = container;
        _contentManager = contentManager;
        return this;
    }
    
    public MapDisplayElementBuilder WithMap(MapFile mapFile)
    {
        _file = mapFile;
        return this;
    }
    
    public MapDisplayElementBuilder WithTemplate(IGameTemplate template)
    {
        _gameTemplate = template;
        return this;
    }

    public MapDisplayElement Build()
    {
        var layers = new List<LayerDisplayElement>();

        foreach (var layer in _file.Layers)
        {
            var builder = new LayerDisplayElementBuilder()
                .WithServices(_container, _contentManager)
                .WithMap(_file)
                .WithLayer(layer)
                .WithTemplate(_gameTemplate);

            layers.Add(builder.Build());
        }
        
        return new MapDisplayElement(layers, _file.BackgroundColor.AsPremultiplied());
    }
}