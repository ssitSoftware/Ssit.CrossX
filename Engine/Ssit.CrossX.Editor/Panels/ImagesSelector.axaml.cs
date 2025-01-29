using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Ssit.CrtossX.Editor.Controls;
using Ssit.CrtossX.Editor.ViewModels;

namespace Ssit.CrtossX.Editor.Panels;

public partial class ImagesSelector : UserControl
{
    public static readonly StyledProperty<ImageSelectorViewModel> ViewModelProperty =
        AvaloniaProperty.Register<SelectionToggleButton, ImageSelectorViewModel>(nameof(ViewModel),
            defaultBindingMode: BindingMode.TwoWay);
    
    public ImageSelectorViewModel ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    
    public ImagesSelector()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        ViewModel = DataContext as ImageSelectorViewModel;
    }
}