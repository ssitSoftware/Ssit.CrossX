using Newtonsoft.Json;
using Ssit.CrossX.Utils;

namespace RetroGunslinger.Core;

public class Settings: AppSettingsBase<Settings>
{
    private bool _cameraShake;
    private int _musicVolume;
    private int _soundVolume;
    private int _language;
    private bool _fullscreen;
    private int _scale = 1;
    private int _palette;
    private int _crtMode;

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
    
    [JsonProperty]
    public bool Fullscreen
    {
        get => _fullscreen;
        set => SetField(ref _fullscreen, value);
    }
    
    [JsonProperty]
    public int Scale
    {
        get => _scale;
        set
        {
            if (value > 5)
            {
                value = 1;
            }

            if (value < 1)
            {
                value = 5;
            }
            
            SetField(ref _scale, value);
        }
    }

    [JsonProperty]
    public int Palette
    {
        get => _palette;
        set => SetField(ref _palette, value);
    }
    
    [JsonProperty]
    public int CrtMode
    {
        get => _crtMode;
        set => SetField(ref _crtMode, value);
    }
}