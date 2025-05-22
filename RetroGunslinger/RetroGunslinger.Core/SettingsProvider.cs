using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IO;

namespace RetroGunslinger.Core;

[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Possible")]
public class SettingsProvider : ISettingsProvider
{
    private readonly IMusicPlayer _musicPlayer;
    private readonly ISoundManager _soundManager;
    private readonly IGameTemplate _gameTemplate;
    private readonly IAppWindowManager _windowManager;
    private readonly IPaletteSource _paletteSource;
    private readonly IActionScheduler _actionScheduler;

    public Settings Settings { get; }
    
    public SettingsProvider(IMusicPlayer musicPlayer, ISoundManager soundManager, IFileStorage fileStorage, 
        IGameTemplate gameTemplate, IAppWindowManager windowManager,
        IPaletteSource paletteSource, IActionScheduler actionScheduler)
    {
        _musicPlayer = musicPlayer;
        _soundManager = soundManager;
        _gameTemplate = gameTemplate;
        _windowManager = windowManager;
        _paletteSource = paletteSource;
        _actionScheduler = actionScheduler;

        Settings = Settings.Load(fileStorage, "settings");
        Settings.PropertyChanged += UpdateSettings;
        
        UpdateSettings(this, new PropertyChangedEventArgs(nameof(Settings.MusicVolume)));
        UpdateSettings(this, new PropertyChangedEventArgs(nameof(Settings.SoundVolume)));
        UpdateSettings(this, new PropertyChangedEventArgs(nameof(Settings.Palette)));

        actionScheduler.Schedule(() =>
        {
            if (Settings.Fullscreen)
            {
                _windowManager.SetFullscreen();
            }
            else
            {
                _windowManager.SetWindowed(_gameTemplate.TargetSize * Settings.Scale);
            }
        });
    }
    
    private void UpdateSettings(object sender, PropertyChangedEventArgs args)
    {
        switch (args.PropertyName)
        {
            case nameof(Settings.MusicVolume):
                _musicPlayer.Volume = Settings.MusicVolume / 8f;
                break;
                
            case nameof(Settings.SoundVolume):
                _soundManager.MasterVolume = Settings.SoundVolume / 4f;
                break;
            
            case nameof(Settings.Fullscreen):
            case nameof(Settings.Scale):
                if (Settings.Fullscreen)
                {
                    _windowManager.SetFullscreen();
                }
                else
                {
                    _windowManager.SetWindowed(_gameTemplate.TargetSize * Settings.Scale);
                }
                break;
            
            case nameof(Settings.Palette):
                _actionScheduler.Schedule(() =>
                    _paletteSource.UpdatePalette(Palette.Palettes[Settings.Palette].Colors));
                break;
        }
    }
}