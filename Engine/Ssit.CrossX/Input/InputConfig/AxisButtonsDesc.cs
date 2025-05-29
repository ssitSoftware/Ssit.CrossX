using Newtonsoft.Json;

namespace Ssit.CrossX.Input.InputConfig;

public class AxisButtonsDesc<T>
{
    [JsonProperty("negative")]
    public T Negative { get; set; }
    
    [JsonProperty("positive")]
    public T Positive { get; set; }
}