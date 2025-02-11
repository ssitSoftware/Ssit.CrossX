using Ssit.CrossX.Audio;
using Ssit.CrossX.Common.Services;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

public class OptionsPageInGameViewModel(
    INavigation navigation,
    ITranslator translator,
    ISoundManager soundManager,
    IMusicPlayer musicPlayer,
    ISettingsProvider settingsProvider,
    IUiSounds sounds,
    ISimulation simulation)
    : OptionsPageViewModel(navigation, translator, soundManager, musicPlayer, settingsProvider, sounds)
{
    public ISimulation Simulation { get; } = simulation;
}