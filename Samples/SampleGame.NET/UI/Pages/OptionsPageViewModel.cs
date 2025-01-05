using System.Windows.Input;
using SampleGame.Services;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Commands;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace SampleGame.UI.Pages;

public class OptionsPageViewModel: IPageCommandsSource
{
    private readonly ITranslator _translator;
    private readonly ISoundManager _soundManager;
    private readonly IMusicPlayer _musicPlayer;
    private readonly IGameSettings _gameSettings;
    ICommand IPageCommandsSource.MenuCommand => null;
    
    public ICommand BackCommand { get; }
    public ICommand SoundVolumeCommand { get; }
    public ICommand MusicVolumeCommand { get; }
    public ICommand CameraShakeCommand { get; }
    public ICommand AutoReloadCommand { get; }
    public ICommand ControlsCommand { get; }
    
    public SharedStringSource SoundVolumeStr { get; } = new();
    public SharedStringSource MusicVolumeStr { get; } = new();
    public SharedStringSource CameraShakeStr { get; } = new();
    public SharedStringSource AutoReloadStr { get; } = new();

    public OptionsPageViewModel(INavigation navigation, ITranslator translator, ISoundManager soundManager, IMusicPlayer musicPlayer, IGameSettings gameSettings)
    {
        _translator = translator;
        _soundManager = soundManager;
        _musicPlayer = musicPlayer;
        _gameSettings = gameSettings;
        
        BackCommand = new SyncCommand(navigation.NavigateBack);
        
        SoundVolumeCommand = new SyncCommand(o =>
        {
            int vol = (int)(soundManager.MasterVolume * 4);
            vol++;
            vol %= 5;
            soundManager.MasterVolume = vol / 4f;
            
            UpdateStrings();
        });
        
        MusicVolumeCommand = new SyncCommand(o =>
        {
            int vol = (int)(musicPlayer.Volume * 4);
            vol++;
            vol %= 5;
            musicPlayer.Volume = vol / 4f;
            
            UpdateStrings();
        });
        
        CameraShakeCommand = new SyncCommand(o =>
        {
            gameSettings.CameraShake = !gameSettings.CameraShake;
            UpdateStrings();
        });
        
        AutoReloadCommand = new SyncCommand(o =>
        {
            gameSettings.AutoReload = !gameSettings.AutoReload;
            UpdateStrings();
        });

        ControlsCommand = new SyncCommand(() => { });
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

        CameraShakeStr.SetSource(_gameSettings.CameraShake ? _translator["Yes"] : _translator["No"]);
        AutoReloadStr.SetSource(_gameSettings.AutoReload ? _translator["Yes"] : _translator["No"]);
    }
}