using Newtonsoft.Json;
using Ssit.CrossX.Input.InputConfig;

namespace Nokemono.Core.Configuration;

public class JsonConfig
{
    [JsonProperty("palettes")]
    public JsonPalette[] Palettes { get; set; }
    
    [JsonProperty("input-mapping")]
    public MappingDesc InputMapping { get; set; }
}