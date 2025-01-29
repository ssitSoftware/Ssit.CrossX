using Avalonia.Controls;
using Avalonia.Interactivity;
using Ssit.CrtossX.Editor.Models.Parameters;

namespace Ssit.CrtossX.Editor.Controls;

public partial class PropertiesControl : UserControl
{
    public PropertiesControl()
    {
        InitializeComponent();
    }

    private void InputElement_OnLostFocus(object sender, RoutedEventArgs e)
    {
        ((sender as TextBox)?.DataContext as ParameterStringModel)?.FocusLostCommand?.Execute(null);
    }
}