using System.Windows.Input;
using SampleGame.Services;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace Gunslinger.Core.UI.ViewModels;

public class OptionsPageViewModel: IPageCommandsSource
{
    private readonly ITranslator _translator;
    private readonly ISoundManager _soundManager;
    private readonly IMusicPlayer _musicPlayer;
    private readonly Settings _settings;
    
    ICommand IPageCommandsSource.MenuCommand => null;
    
    public ICommand BackCommand { get; }
    public ICommand SoundVolumeCommand { get; }
    public ICommand MusicVolumeCommand { get; }
    public ICommand CameraShakeCommand { get; }
    public ICommand ControlsCommand { get; }
    public ICommand LanguageCommand { get; }
    
    public SharedStringSource SoundVolumeStr { get; } = new();
    public SharedStringSource MusicVolumeStr { get; } = new();
    public SharedStringSource CameraShakeStr { get; } = new();

    public OptionsPageViewModel(INavigation navigation, ITranslator translator, ISoundManager soundManager,
        IMusicPlayer musicPlayer, ISettingsProvider settingsProvider, IUiSounds sounds)
    {
        _translator = translator;
        _soundManager = soundManager;
        _musicPlayer = musicPlayer;
        _settings = settingsProvider.Settings;

        BackCommand = new SyncCommand( () =>
        {
            sounds[UiSounds.NavigateBackSound]?.PlayOnce();
            navigation.NavigateBack();
        });
    
        SoundVolumeCommand = new SyncCommand(o =>
        {
            int vol = (int)(soundManager.MasterVolume * 4);
            vol++;
            vol %= 5;
            _settings.SoundVolume = vol;
            _settings.Save();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            UpdateStrings();
        });
        
        MusicVolumeCommand = new SyncCommand(o =>
        {
            int vol = (int)(musicPlayer.Volume * 4);
            vol++;
            vol %= 5;
            _settings.MusicVolume = vol;
            _settings.Save();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            UpdateStrings();
        });
        
        CameraShakeCommand = new SyncCommand(o =>
        {
            _settings.CameraShake = !_settings.CameraShake;
            _settings.Save();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            UpdateStrings();
        });
        
        LanguageCommand = new SyncCommand( ()=>
        {
            translator.ToggleLanguage();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
        });

        ControlsCommand = new SyncCommand(() =>
        {
            sounds[UiSounds.NavigateToSound]?.PlayOnce();
        });
        UpdateStrings();
    }
    
    private void UpdateStrings()
    {
        int vol = (int)(_soundManager.MasterVolume * 4);
        vol *= 25;
        
        if (vol == 0)
        {
            SoundVolumeStr.SetSource(_translator["Off"]);
        }
        else
        {
            SoundVolumeStr.SetSource($"{vol}%");
        }

        vol = (int)(_musicPlayer.Volume * 4);
        vol *= 25;
        
        if (vol == 0)
        {
            MusicVolumeStr.SetSource(_translator["Off"]);
        }
        else
        {
            MusicVolumeStr.SetSource($"{vol}%");
        }

        CameraShakeStr.SetSource(_settings.CameraShake ? _translator["Yes"] : _translator["No"]);
    }
}