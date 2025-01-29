using System;
using Avalonia.Controls;
using Ssit.CrtossX.Editor.Helpers;

namespace Ssit.CrtossX.Editor.Views;

public partial class ResizeLayerDialog : Window
{
    public ResizeLayerDialog()
    {
        InitializeComponent();
    }
    
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        WindowHelpers.FitDialog(this, Panel);
    }
}