using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ssit.CrossX.Games.Logic;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
public class ObjectDescription
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    private class Event
    {
        public string Name { get; set; }
        public string Sequence { get; set; }
        public int Frame { get; set; }
        public JObject Parameters { get; set; }
    }
    
    private class Desc
    {
        public Vector2 Origin { get; set; }
        public SizeF Size { get; set; }
        public Event[] Events { get; set; }
    }
    
    public Vector2 Origin { get; }
    public SizeF Size { get; }
    public Size SourceSize { get; }
    public ObjectEventDescription[] Events { get; }

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
        Events = obj.Events?.Select(o => new ObjectEventDescription(o.Name, o.Sequence, o.Frame, o.Parameters)).ToArray();
    }
}

