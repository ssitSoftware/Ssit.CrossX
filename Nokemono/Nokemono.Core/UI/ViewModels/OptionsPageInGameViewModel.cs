using Nokemono.Core.Game;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.UI.Common.Services;
using Ssit.CrossX.UI.Services;

namespace Nokemono.Core.UI.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global
public class OptionsPageInGameViewModel(
    INavigation navigation,
    ITranslator translator,
    ISettingsProvider settingsProvider,
    IUiSounds sounds,
    IGameInterfaces gameInterfaces)
    : OptionsPageViewModel(navigation, translator, settingsProvider, sounds)
{
    public IGameInterfaces GameInterfaces { get; } = gameInterfaces;
}