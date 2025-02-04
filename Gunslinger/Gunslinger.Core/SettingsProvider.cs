using System.ComponentModel;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Common.Services;
using Ssit.CrossX.IO;

namespace Gunslinger.Core;

public class SettingsProvider : ISettingsProvider
{
    private readonly IMusicPlayer _musicPlayer;
    private readonly ISoundManager _soundManager;
    private readonly ITranslator _translator;

    public Settings Settings { get; }
    
    public SettingsProvider(IMusicPlayer musicPlayer, ISoundManager soundManager, IFileStorage fileStorage)
    {
        _musicPlayer = musicPlayer;
        _soundManager = soundManager;

        Settings = Settings.Load(fileStorage, "Gunslinger/settings");
        Settings.PropertyChanged += UpdateSettings;
        
        UpdateSettings(this, new PropertyChangedEventArgs(nameof(Settings.MusicVolume)));
        UpdateSettings(this, new PropertyChangedEventArgs(nameof(Settings.SoundVolume)));
    }
    
    private void UpdateSettings(object sender, PropertyChangedEventArgs args)
    {
        switch (args.PropertyName)
        {
            case nameof(Settings.MusicVolume):
                _musicPlayer.Volume = Settings.MusicVolume / 4f;
                break;
                
            case nameof(Settings.SoundVolume):
                _soundManager.MasterVolume = Settings.SoundVolume / 4f;
                break;
        }
    }
}