using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Views;

public class LabelRadio : LabelButton
{
    public SharedValue<int> SelectedValue { get; set; }
    public int Value { get; set; }
}