using Newtonsoft.Json;

namespace Ssit.CrossX.Graphics.Sprites.Json;

internal class JsonSprite
{
    [JsonProperty("frames")]
    public JsonFrame[] Frames { get; set; }
}