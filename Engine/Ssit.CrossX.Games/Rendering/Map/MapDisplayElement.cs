using System;
using System.Collections.Generic;

namespace Ssit.CrossX.Games.Rendering.Map;

public class MapDisplayElement: IDisposable
{
    public IReadOnlyList<LayerDisplayElement> Layers => _layers;
    private readonly List<LayerDisplayElement> _layers;
    
    internal MapDisplayElement(List<LayerDisplayElement> layers)
    {
        _layers = layers;
    }
    
    public void Dispose()
    {
        foreach (var layer in Layers)
        {
            layer.Dispose();
        }
        _layers.Clear();
    }
}