using System.Collections.Generic;
using System.Linq;
using Ssit.CrossX.Editor.Helpers;
using SkiaSharp;
using Ssit.CrossX.Games;
using Ssit.CrossX.XxFormats.Template;

namespace Ssit.CrossX.Editor.Service
{
    public class TilesetsContainer: ITilesetsContainer
    {
        private readonly IGameTemplate _gameTemplate;

        public Tileset[] TileSets { get; private set; }

        public TilesetsContainer(IGameTemplate gameTemplate)
        {
            _gameTemplate = gameTemplate;
        }
    
        public void Load()
        {
            var list = new List<Tileset>();
        
            var paths = _gameTemplate.TileSets.ToArray();

            foreach (var path in paths)
            {
                using var stream = _gameTemplate.AssetsProvider.Open(path);
                var image = SKImage.FromEncodedData(stream);

                var cutPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path)!,
                    System.IO.Path.GetFileNameWithoutExtension(path));
                
                using var metaStream = _gameTemplate.AssetsProvider.Open(cutPath + ".tiles");
                var meta = TilesetMeta.FromStream(metaStream);
                
                list.Add(new Tileset(image, meta, path));
            }

            TileSets = list.ToArray();
        }
    }
}