using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.IO;
using Ssit.CrossX.Utils;
using Ssit.CrossX.XxFormats.Map;
using Ssit.CrossX.XxFormats.Template;
using Ssit.CrossX.XxGames.AabbPhysics;
using Ssit.CrossX.XxGames.AabbPhysics.Colliders;
using Ssit.CrossX.XxGames.Algorithms;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Coliders;
using Ssit.IoC;

namespace Ssit.CrossX.XxGames.Platformer.Builders;

public class AabbSimulationBuilder
{
    private class Matcher : RegionsFromArray<IMaterial>.IMatcher
    {
        public bool Match(IMaterial origin, int x, int y, IMaterial value)
        {
            return ReferenceEquals(origin, value);
        }

        public bool IsEmpty(IMaterial value)
        {
            return value is null;
        }

        public IMaterial Empty => null;
    }
    
    private MapFile _mapFile;
    private IFilesProvider _filesProvider;
    private IGameTemplate _gameTemplate;
    private IIoCContainer _container;
    private Action<IIoCContainerBuilder> _registerServices;

    private IMaterial[] _materials;
    
    public AabbSimulationBuilder WithContainer(IIoCContainer container)
    {
        _container = container;
        return this;
    }
    
    public AabbSimulationBuilder WithGameTemplate(IGameTemplate gameTemplate)
    {
        _gameTemplate = gameTemplate;
        return this;
    }
    
    public AabbSimulationBuilder WithMaterials(IMaterial[] materials)
    {
        _materials = materials;
        return this;
    }
    
    public AabbSimulationBuilder WithFilesProvider(IFilesProvider filesProvider)
    {
        _filesProvider = filesProvider;
        return this;
    }
    
    public AabbSimulationBuilder WithMap(MapFile file)
    {
        _mapFile = file;
        return this;
    }
    
    public AabbSimulationBuilder WithServicesRegistrar(Action<IIoCContainerBuilder> registerServices)
    {
        _registerServices = registerServices;
        return this;
    }

    public (ISimulation, IIoCContainer) Build()
    {
        var servicesBuilder = IoC.IoC.NewBuilder();
        servicesBuilder.WithParent(_container);
        
        var simulation = new Simulation();
        simulation.SimulationParameters.GravityAcceleration = _gameTemplate.Gravity;

        servicesBuilder.WithInstance<ISimulation>(simulation);
        servicesBuilder.WithInstance(_gameTemplate);
        
        _registerServices?.Invoke(servicesBuilder);
        
        var container = servicesBuilder.Build();
        
        var tilesets = _mapFile.Tilesets.Select(o =>
        {
            using var stream = _filesProvider.Open(PathHelper.GetPathWithExtension(o, "mask.png"));
            return ImagesUtility.LoadImage(stream);

        }).ToArray();

        var width = _mapFile.MainLayer.Width * _gameTemplate.TileSize;
        var height = _mapFile.MainLayer.Height * _gameTemplate.TileSize;
        
        var collisions = new IMaterial[width, height];

        List<MapLayer> layers = [
            _mapFile.MainLayer
        ];

        foreach (var layer in layers)
        {
            for (var ix = 0; ix < layer.Width; ++ix)
            {
                for (var iy = 0; iy < layer.Height; ++iy)
                {
                    var tile = layer.Tiles[ix, iy];
                    if (tile.IsEmpty)
                    {
                        continue;
                    }

                    FillCollisions(tilesets[tile.TileSet], tile, collisions, ix, iy, _gameTemplate.TileSize);
                }
            }
        }

        GenerateStaticColliders(collisions, simulation);
        
        try
        {
            GenerateObjects(_mapFile.MainLayer.Objects, container);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return (simulation, container);
    }
    
    private void GenerateStaticColliders(IMaterial[,] collisions, Simulation simulation)
    {
        var list = new List<RectCollider>();
        var regionsFromArray = new RegionsFromArray<IMaterial>();
        
        // ReSharper disable PossibleLossOfFraction
        var mapAabb = new Aabb(0, 0, collisions.GetLength(0) / _gameTemplate.TileSize, collisions.GetLength(1) / _gameTemplate.TileSize);

        regionsFromArray.GenerateRegions(collisions, null, new Matcher(), CreateCollider, list);
        simulation.InitializeStaticColliders(mapAabb, list);
    }

    private void CreateCollider(IMaterial material, int x, int y, int width, int height, object listAsObject)
    {
        var list = (List<RectCollider>)listAsObject;
        var xx = (float)x / _gameTemplate.TileSize;
        var yy = (float)y / _gameTemplate.TileSize;
        var ww = (float)width / _gameTemplate.TileSize;
        var hh = (float)height / _gameTemplate.TileSize;

        var cx = xx + ww / 2;
        var cy = yy + hh / 2;

        list.Add(new RectCollider(new RectColliderCreationParameters
        {
            Type = ColliderType.Static,
            Center = new Vector2(cx, cy),
            Size = new Vector2(ww, hh),
            Material = material,
            Active = true
        }));
    }
    
    private void FillCollisions(RgbaColor[,] tileset, Tile tile, IMaterial[,] collisions, int ix, int iy, int tileSize)
    {
        var material = _materials[tile.Material];
        
        var tx = tile.X * tileSize;
        var ty = tile.Y * tileSize;

        for (var x = 0; x < tileSize; ++x)
        {
            for (var y = 0; y < tileSize; ++y)
            {
                if (tileset[tx + x, ty + y].A > 32)
                {
                    collisions[ix * tileSize + x, iy * tileSize + y] = material;
                }
            }
        }
    }

    private void GenerateObjects(List<MapObject> mainLayerObjects, IIoCContainer container)
    {
        var linkMap = new LinkMap();
        
        foreach (var obj in mainLayerObjects)
        {
            if (!obj.HasLogic) continue;

            ObjectCreationParameters parameters = null;

            if (obj.ParametersObject != null)
            {
                var paramType = obj.ParametersObject.GetType();
                var parametersType = typeof(ObjectCreationParameters<>).MakeGenericType(paramType);
                parameters = (ObjectCreationParameters)Activator.CreateInstance(parametersType);
            }
            
            parameters ??= new ObjectCreationParameters();
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