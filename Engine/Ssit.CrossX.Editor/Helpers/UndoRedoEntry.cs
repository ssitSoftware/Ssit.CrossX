using System;
using System.IO;
using System.IO.Compression;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Editor.Service;

namespace Ssit.CrossX.Editor.Helpers;

public class UndoRedoEntry
{
    private readonly MapFile _mapFile;
    private readonly IEditorInstances _editorInstances;
    private readonly byte[] _data;

    public UndoRedoEntry(IEditorInstances editorInstances)
    {
        _mapFile = editorInstances.Map;
        _editorInstances = editorInstances;

        var memoryStream = new MemoryStream();
        var gzipStream = new GZipStream(memoryStream, CompressionLevel.SmallestSize);
        var writer = new BinaryWriter(gzipStream);

        _mapFile.SaveRaw(writer);
        gzipStream.Flush();
        memoryStream.Flush();

        _data = memoryStream.GetBuffer();
    }

    public void Restore()
    {
        var selectedLayerIndex = _mapFile.Layers.IndexOf( _editorInstances.Editor.SelectedLayer);
        
        var mamoryStream = new MemoryStream(_data);
        var gzipStream = new GZipStream(mamoryStream, CompressionMode.Decompress);
        var reader = new BinaryReader(gzipStream);
        
        _mapFile.LoadRaw(reader, _editorInstances.Template);

        var obj = _mapFile.FindObject(_editorInstances.Editor.SelectedObject);
        if (obj == null)
        {
            _editorInstances.Editor.SelectedObject = 0;
        }
        
        _editorInstances.Editor.SelectedLayer = _mapFile.Layers[Math.Min(_mapFile.Layers.Count-1, selectedLayerIndex)];
    }
}