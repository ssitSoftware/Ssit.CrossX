using Newtonsoft.Json;
using Ssit.CrossX.Utils;

namespace Gunslinger.Core;

public class Settings: AppSettingsBase<Settings>
{
    private bool _cameraShake;
    private int _musicVolume;
    private int _soundVolume;
    private int _language;

    [JsonProperty]
    public bool CameraShake
    {
        get => _cameraShake;
        set => SetField(ref _cameraShake, value);
    }

    [JsonProperty]
    public int MusicVolume
    {
        get => _musicVolume;
        set => SetField(ref _musicVolume, value);
    }

    [JsonProperty]
    public int SoundVolume
    {
        get => _soundVolume;
        set => SetField(ref _soundVolume, value);
    }

    [JsonProperty]
    public int Language
    {
        get => _language;
        set => SetField(ref _language, value);
    }
}