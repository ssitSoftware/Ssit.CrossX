using Ssit.CrossX.UI.Parameters;

namespace Ssit.CrossX.UI.Views;

public class LabelButtonEx: LabelButton
{
    public Length? FocusBevel { get; set; }
    public Length? FocusWaveAmplitude { get; set; }
    public float? FocusWaveFrequency { get; set; }
    public ColorWrapper FocusedLowColor { get; set; }
}