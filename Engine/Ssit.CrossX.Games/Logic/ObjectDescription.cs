using System.Numerics;
using Newtonsoft.Json;

namespace Ssit.CrossX.Games.Logic;

public class ObjectDescription
{
    private class Desc
    {
        public Vector2 Origin { get; set; }
        public SizeF Size { get; set; }
    }
    
    public Vector2 Origin { get; }
    public SizeF Size { get; }
    public Size SourceSize { get; }

    public ObjectDescription(Vector2 origin, Size sourceSize)
    {
        Origin = origin;
        SourceSize = sourceSize;
        Size = SizeF.Empty;
    }
    
    public ObjectDescription(string json, Size sourceSize)
    {
        var obj = JsonConvert.DeserializeObject<Desc>(json);
        
        Origin = obj.Origin;
        Size = obj.Size;
        SourceSize = sourceSize;
    }
}