using System;
using Avalonia.Controls;
using Ssit.CrossX.Editor.Helpers;

namespace Ssit.CrossX.Editor.Views;

public partial class ResizeLayerDialog : Window
{
    public ResizeLayerDialog()
    {
        InitializeComponent();
    }
    
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        WindowHelpers.FitDialog(this, Pnl);
    }
}