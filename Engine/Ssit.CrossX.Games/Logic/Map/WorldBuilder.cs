using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Common;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.IO;
using Ssit.IoC;

namespace Ssit.CrossX.Games.Logic.Map;

public class WorldBuilder
{
    private MapFile _mapFile;
    private IFilesProvider _filesProvider;
    private IGameTemplate _gameTemplate;
    private IIoCContainer _container;
    private Action<IIoCContainerBuilder> _registerServices;

    public WorldBuilder WithContainer(IIoCContainer container)
    {
        _container = container;
        return this;
    }
    
    public WorldBuilder WithGameTemplate(IGameTemplate gameTemplate)
    {
        _gameTemplate = gameTemplate;
        return this;
    }
    
    public WorldBuilder WithFilesProvider(IFilesProvider filesProvider)
    {
        _filesProvider = filesProvider;
        return this;
    }
    
    public WorldBuilder WithMap(MapFile file)
    {
        _mapFile = file;
        return this;
    }
    
    public WorldBuilder WithServicesRegistrar(Action<IIoCContainerBuilder> registerServices)
    {
        _registerServices = registerServices;
        return this;
    }

    public (World, IIoCContainer) Build()
    {
        var servicesBuilder = IoC.IoC.NewBuilder();
        servicesBuilder.WithParent(_container);
        
        var world = new World(_gameTemplate.Gravity);

        servicesBuilder.WithInstance(world);
        servicesBuilder.WithSingleton<GameObjectsServices, GameObjectsServices>();
        servicesBuilder.WithSingleton<ICommonSoundContainer, CommonSoundContainer>();
        
        servicesBuilder.WithInstance(_gameTemplate);
        servicesBuilder.WithSingleton<ICamera, Camera>();
        
        _registerServices?.Invoke(servicesBuilder);
        
        var container = servicesBuilder.Build();
        
        var tilesets = _mapFile.Tilesets.Select(o =>
        {
            using var stream = _filesProvider.Open(PathHelper.GetPathWithExtension(o, "tiles"));
            return TilesetMeta.FromStream(stream);
        }).ToArray();

        var layer = _mapFile.MainLayer;
        var collisions = new Dictionary<string, List<Vector2[]>>();

        for (var ix = 0; ix < layer.Width; ++ix)
        {
            for (var iy = 0; iy < layer.Height; ++iy)
            {
                var tile = layer.Tiles[ix, iy];
                if (tile.IsEmpty)
                {
                    continue;
                }

                var col = tilesets[tile.TileSet].GetCollisionPolygon(tile.X, tile.Y);
                var material = _gameTemplate.Materials[tile.Material].Name;

                if (col == null || material == null)
                {
                    continue;
                }

                var offset = new Vector2(ix, iy);
                col = col.Select( o=> o + offset).ToArray();

                if (!collisions.TryGetValue(material, out var list))
                {
                    list = new List<Vector2[]>();
                    collisions.Add(material, list);
                }

                list.Add(col);
            }
        }

        var materials = _gameTemplate.Materials.ToList();
        
        foreach (var pair in collisions)
        {
            var lines = PolygonMerge.Merge(pair.Value, 1000, 1000);
            var staticBody = new Body(world, Vector2.Zero);
            
            staticBody.MaterialIndex = materials.FindIndex(o=>o.Name == pair.Key);
            
            foreach (var col in lines)
            {
                switch (col.Item1.Length)
                {
                    case 0:
                    case 1:
                        throw new InvalidDataException();
                        
                    case 2:
                        staticBody.CreateFixture(new EdgeShape(col.Item1[0], col.Item1[1]));
                        break;
                    
                    default:
                        staticBody.CreateFixture(new ChainShape(new Vertices(col.Item1), col.Item2));
                        break;
                }
            }
        }

        try
        {
            GenerateObjects(_mapFile.MainLayer.Objects, container);
        }
        catch (Exception e)
        {
            throw;
        }

        
        return (world, container);
    }

    private void GenerateObjects(List<MapObject> mainLayerObjects, IIoCContainer container)
    {
        var linkMap = new LinkMap();
        
        foreach (var obj in mainLayerObjects)
        {
            if (!obj.HasLogic) continue;
            
            ObjectCreationParameters parameters;

            if (obj.ParametersObject == null)
            {
                parameters = new ObjectCreationParameters();
            }
            else
            {
                var paramType = obj.ParametersObject.GetType();
                var parametersType = typeof(ObjectCreationParameters<>).MakeGenericType(paramType);
                parameters = (ObjectCreationParameters)Activator.CreateInstance(parametersType);
            }
            
            parameters.Position = obj.Position;
            parameters.Flipped = obj.Flipped;
            parameters.ParametersObject = obj.ParametersObject;
            parameters.LinkMap = linkMap;
            parameters.ZOrder = obj.ZOrder;
            
            var instance = container.IoCConstruct(obj.Type, parameters);
            linkMap.AddMapping(obj.Id, instance);
        }
        linkMap.ResolveLinks();
    }
}