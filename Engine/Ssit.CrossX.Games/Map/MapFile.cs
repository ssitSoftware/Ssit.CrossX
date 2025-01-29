using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Ssit.CrossX.Games.Template;

namespace Ssit.CrossX.Games.Map;

public class MapFile: BindableModel
{
    const string Header = "BREEZE MAP";
    
    private MapLayer _mainLayer;
    private bool _isModified;
    private const int GuidLength = 16;

    public string[] Tilesets { get; }
    
    public Guid TemplateId { get; }
    public int TileSize { get; }

    public int NextObjectId
    {
        get
        {
            int nextObjectId = 0;
            for (var idx = 0; idx < Layers.Count; ++idx)
            {
                var layer = Layers[idx];
                for (var idx2 = 0; idx2 < layer.Objects.Count; ++idx2)
                {
                    nextObjectId = Math.Max(layer.Objects[idx2].Id, nextObjectId);
                }
            }

            return nextObjectId + 1;
        }
    }
    
    public ObservableCollection<MapLayer> Layers { get; } = new();
    
    public bool IsModified
    {
        get => _isModified;
        private set => SetField(ref _isModified, value);
    }

    public MapLayer MainLayer
    {
        get
        {
            if (_mainLayer != null)
                return _mainLayer;
            
            return _mainLayer = Layers.FirstOrDefault(o => o.Name.ToLowerInvariant() == "main") ??
                         Layers.FirstOrDefault(o => o.HorizontalSpeed == 1 && o.VerticalSpeed == 1) ??
                         Layers.First();
        }
    }

    public MapFile(Guid templateId, int tileSize, string[] tilesets)
    {
        TemplateId = templateId;
        TileSize = tileSize;
        Tilesets = tilesets;
    }

    public static Guid ReadTemplateId(BinaryReader reader)
    {
        var bytes = reader.ReadBytes(Header.Length);

        for (var idx = 0; idx < bytes.Length; ++idx)
        {
            if (Header[idx] != bytes[idx]) throw new InvalidDataException("Not Breeze Map File!");
        }

        var guid = reader.ReadBytes(GuidLength);
        var templateId = new Guid(guid);
        return templateId;
    }
    
    public static MapFile FromStream(Stream stream, IGameTemplate gameTemplate, string[] tilesetsOverride = null)
    {
        var gzipStream = new GZipStream(stream, CompressionMode.Decompress);
        var reader = new BinaryReader(gzipStream);
        
        var templateId = ReadTemplateId(reader);

        var tileSize = reader.ReadByte();
        
        var tilesetCount = reader.ReadByte();
        var tilesets = new string[tilesetCount]; 
            
        for (var idx = 0; idx < tilesetCount; ++idx)
        {
            tilesets[idx] = reader.ReadString();
        }
        
        var mapFile = new MapFile(templateId, tileSize, tilesetsOverride ?? tilesets);
        mapFile.LoadRaw(reader, gameTemplate);

        if (tilesetsOverride != null)
        {
            var mapping = new int[tilesets.Length];

            for (var idx = 0; idx < tilesets.Length; ++idx)
            {
                var index = Array.IndexOf(tilesetsOverride, tilesets[idx]);
                mapping[idx] = index;
            }

            foreach (var layer in mapFile.Layers)
            {
                layer.RemapTilesets(mapping);
            }
        }
        
        return mapFile;
    }

    public void OnModified()
    {
        IsModified = true;
        for (var idx = 0; idx < Layers.Count; ++idx)
        {
            Layers[idx].Objects.Sort((o1, o2) => o1.ZOrder - o2.ZOrder);
        }
    }

    public void Save(Stream stream, bool compress = true)
    {
        BinaryWriter writer;
        GZipStream gzipStream = null;

        if (compress)
        {
            gzipStream = new(stream, CompressionLevel.Optimal);
            writer = new(gzipStream);
        }
        else
        {
            writer = new(stream);
        }
        
        var headerBytes = Encoding.ASCII.GetBytes(Header);

        writer.Write(headerBytes);
        writer.Write(TemplateId.ToByteArray());
        writer.Write((byte)TileSize);

        writer.Write((byte)Tilesets.Length);
        for (var idx = 0; idx < Tilesets.Length; ++idx)
        {
            writer.Write(Tilesets[idx]);
        }
        
        SaveRaw(writer);

        stream.Flush();
        gzipStream?.Flush();
        gzipStream?.Dispose();
        IsModified = false;
    }
    
    public void LoadRaw(BinaryReader reader, IGameTemplate gameTemplate)
    {
        Layers.Clear();
        
        var layersNo = reader.ReadByte();
        for (var idx = 0; idx < layersNo; ++idx)
        {
            var layer = new MapLayer();
            layer.Load(reader, gameTemplate);
            Layers.Add(layer);
        }

        _mainLayer = null;
    }
    
    public void SaveRaw(BinaryWriter writer)
    {
        writer.Write((byte)Layers.Count);
        foreach (var layer in Layers)
        {
            layer.Save(writer);
        }
    }

    public MapObject FindObject(int id)
    {
        for (var idx = 0; idx < Layers.Count; ++idx)
        {
            var layer = Layers[idx];

            var obj = layer.FindObject(id);
            if (obj != null)
            {
                return obj;
            }
        }

        return null;
    }

    public void DeleteObject(int id)
    {
        for (var idx = 0; idx < Layers.Count; ++idx)
        {
            var layer = Layers[idx];
            for (var idx2 = 0; idx2 < layer.Objects.Count; ++idx2)
            {
                if (layer.Objects[idx2].Id == id)
                {
                    layer.Objects.RemoveAt(idx2);
                    OnModified();
                    return;
                }
            }
        }
    }
}