using Ssit.CrossX.Common.Services;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global
public class OptionsPageInGameViewModel(
    INavigation navigation,
    ITranslator translator,
    ISettingsProvider settingsProvider,
    IUiSounds sounds,
    IGameInstance gameInstance)
    : OptionsPageViewModel(navigation, translator, settingsProvider, sounds)
{
    public IGameInstance GameInstance { get; } = gameInstance;
}