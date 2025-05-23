using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Nokemono.Core.UI.Views;

public class LabelButtonEx: LabelButton
{
    public Length? FocusBevel { get; set; }
    public Length? FocusWaveAmplitude { get; set; }
    public float? FocusWaveFrequency { get; set; }
}