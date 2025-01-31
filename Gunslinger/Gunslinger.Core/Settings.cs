using Newtonsoft.Json;
using Ssit.CrossX.Utils;

namespace Gunslinger.Core;

public interface ISettingsProvider
{
    Settings Settings { get; }
}

public class Settings: AppSettingsBase<Settings>
{
    [JsonProperty] public bool CameraShake { get; set; }
    [JsonProperty] public int MusicVolume { get; set; }
    [JsonProperty] public int SoundVolume { get; set; }
}