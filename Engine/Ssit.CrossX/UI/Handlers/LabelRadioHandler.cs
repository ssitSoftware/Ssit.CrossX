using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelRadioHandler<TLabelRadio>: LabelButtonHandler<TLabelRadio> where TLabelRadio: LabelRadio
{
    protected override bool IsChecked => (AttachedView.SelectedValue?.Value ?? -1) == AttachedView.Value;
    
    public LabelRadioHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IActionDispatcher actionDispatcher,
        IUiSounds uiSounds, IHapticDevice hapticDevice, PageInputContext pageInputContext, IPaletteSource paletteSource = null) 
        : base(parameters, fontsManager, actionDispatcher, uiSounds, hapticDevice, pageInputContext, paletteSource)
    {
    }
}