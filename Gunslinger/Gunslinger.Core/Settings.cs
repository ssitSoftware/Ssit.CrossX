using Newtonsoft.Json;
using Ssit.CrossX.Utils;

namespace Gunslinger.Core;

public interface ISettingsProvider
{
    Settings Settings { get; }
}

public class Settings: AppSettingsBase<Settings>
{
    private bool _cameraShake;
    private int _musicVolume;
    private int _soundVolume;
    private bool _optimize;

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
    public bool Optimize
    {
        get => _optimize;
        set => SetField(ref _optimize, value);
    }
}