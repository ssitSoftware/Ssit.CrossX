using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Common.Services;
using Ssit.CrossX.UI;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;

namespace RetroGunslinger.Core.UI.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global
public class OptionsPageViewModel: IPageCommandsSource
{
    private readonly ITranslator _translator;
    private readonly Settings _settings;
    
    ICommand IPageCommandsSource.MenuCommand => null;
    
    public ICommand BackCommand { get; }
    public ICommand SoundVolumeCommand { get; }
    public ICommand MusicVolumeCommand { get; }
    public ICommand LanguageCommand { get; }
    public ICommand PaletteCommand { get; }
    public ICommand CrtCommand { get; }
    public ICommand FullScreenCommand { get; }
    public SyncCommand ScaleCommand { get; }
    
    public SharedStringSource SoundVolumeStr { get; } = new();
    public SharedStringSource MusicVolumeStr { get; } = new();
    public SharedStringSource PaletteStr { get; } = new();
    public SharedStringSource CrtStr { get; } = new();
    
    public SharedStringSource FullscreenStr { get; } = new();
    
    public SharedStringSource ScaleStr { get; } = new();

    public OptionsPageViewModel(INavigation navigation, ITranslator translator, ISettingsProvider settingsProvider, IUiSounds sounds)
    {
        _translator = translator;
        _settings = settingsProvider.Settings;

        BackCommand = new SyncCommand( () =>
        {
            sounds[UiSounds.NavigateBackSound]?.PlayOnce();
            navigation.NavigateBack();
        });
    
        SoundVolumeCommand = new SyncCommand(o =>
        {
            (_, var type) = o as (object obj, ButtonCommandType type)? ?? (null, ButtonCommandType.Select);
            
            int vol = _settings.SoundVolume;
            
            vol += type == ButtonCommandType.Previous ? -1 : 1;
            vol += 5;
            vol %= 5;
            
            _settings.SoundVolume = vol;
            _settings.Save();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            UpdateStrings();
        });
        
        MusicVolumeCommand = new SyncCommand(o =>
        {
            (_, var type) = o as (object obj, ButtonCommandType type)? ?? (null, ButtonCommandType.Select);
            
            int vol = _settings.MusicVolume;
            
            vol += type == ButtonCommandType.Previous ? -1 : 1;
            vol += 5;
            vol %= 5;
            
            _settings.MusicVolume = vol;
            _settings.Save();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            UpdateStrings();
        });
        
        LanguageCommand = new SyncCommand( o =>
        {
            (_, var type) = o as (object obj, ButtonCommandType type)? ?? (null, ButtonCommandType.Select);
            
            translator.ToggleLanguage(type == ButtonCommandType.Previous);
            _settings.Language = translator.CurrentLanguage;
            _settings.Save();
            
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
        });

        PaletteCommand = new SyncCommand(o =>
        {
            (_, var type) = o as (object obj, ButtonCommandType type)? ?? (null, ButtonCommandType.Select);
            if (type == ButtonCommandType.Previous)
            {
                _settings.Palette = (_settings.Palette + Palette.Palettes.Length - 1) % Palette.Palettes.Length;
            }
            else
            {
                _settings.Palette = (_settings.Palette + 1) % Palette.Palettes.Length;
            }

            _settings.Save();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            UpdateStrings();
        });
        
        CrtCommand = new SyncCommand(o =>
        {
            (_, var type) = o as (object obj, ButtonCommandType type)? ?? (null, ButtonCommandType.Select);
            if (type == ButtonCommandType.Previous)
            {
                _settings.CrtMode = (_settings.CrtMode + 2) % 3;
            }
            else
            {
                _settings.CrtMode = (_settings.CrtMode + 1) % 3;
            }

            _settings.Save();
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            UpdateStrings();
        });

        FullScreenCommand = new SyncCommand(_ =>
        {
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();
            
            _settings.Fullscreen = !_settings.Fullscreen;
            _settings.Save();
            UpdateStrings();
        });

        ScaleCommand = new SyncCommand(o =>
        {
            (_, var type) = o as (object obj, ButtonCommandType type)? ?? (null, ButtonCommandType.Select);
            
            sounds[UiSounds.ChangeValueSound]?.PlayOnce();

            _settings.Scale += type == ButtonCommandType.Previous ? -1 : 1;
            _settings.Save();
            
            UpdateStrings();
        }, _ => !_settings.Fullscreen);
        
        UpdateStrings();
    }
    
    private void UpdateStrings()
    {
        int vol = _settings.SoundVolume * 25;
        
        if (vol == 0)
        {
            SoundVolumeStr.SetSource(_translator["Off"]);
        }
        else
        {
            SoundVolumeStr.SetSource($"{vol}%");
        }

        vol = _settings.MusicVolume * 25;
        
        if (vol == 0)
        {
            MusicVolumeStr.SetSource(_translator["Off"]);
        }
        else
        {
            MusicVolumeStr.SetSource($"{vol}%");
        }

        PaletteStr.SetSource(_translator[Palette.Palettes[_settings.Palette].Name]);
        FullscreenStr.SetSource(_settings.Fullscreen ? _translator["Yes"] : _translator["No"]);
        ScaleStr.SetSource($"{_settings.Scale}x");
        CrtStr.SetSource(_translator[$"Crt{_settings.CrtMode}"]);
        ScaleCommand.RaiseCanExecuteChanged();
    }
}