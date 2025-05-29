using Newtonsoft.Json;

namespace Nokemono.Core.Configuration;

public class JsonPalette
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("colors")]
    public string[] Colors { get; set; }
    
    [JsonProperty("button-accent-color")]
    public int ButtonAccentColor { get; set; }
}