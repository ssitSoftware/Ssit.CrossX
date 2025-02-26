using System;
using System.Windows.Input;
using Ssit.CrossX.Editor.Input;
using Ssit.CrossX.Editor.Service;
using CommunityToolkit.Mvvm.Input;
using Ssit.CrossX.Games.Map;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.Editor.ViewModels;

public class ResizeLayerDialogViewModel: BindableModel, IDialog
{
    private readonly MapLayer _layer;
    private readonly IEditorInstances _instances;
    private int _width;
    private int _height;
    private MapAlign _align;

    public event Action RequestClose;
    
    public string Title { get; }

    public MapAlign Align
    {
        get => _align;
        set => SetField(ref _align, value);
    }
    
    public MapAlign[] Aligns { get; }

    public int Width
    {
        get => _width;
        set => SetField(ref _width, value);
    }

    public int Height
    {
        get => _height;
        set => SetField(ref _height, value);
    }
    
    public ICommand CloseCommand { get; }
    public ICommand ApplyCommand { get; }

    public ResizeLayerDialogViewModel(MapLayer layer, IEditorInstances instances)
    {
        _layer = layer;
        _instances = instances;
        Title = $"Resize {layer.Name} layer";

        Width = _layer.Width;
        Height = _layer.Height;

        Aligns = new[]
        {
            MapAlign.Left, MapAlign.Center, MapAlign.Right,
            MapAlign.VCenter, MapAlign.VCenterCenter, MapAlign.VCenterRight,
            MapAlign.Bottom, MapAlign.BottomCenter, MapAlign.BottomRight
        };
        Align = MapAlign.Bottom;

        CloseCommand = new RelayCommand(() => RequestClose?.Invoke());
        ApplyCommand = new RelayCommand(Apply);
    }
    
    private void Apply()
    {
        _instances.UndoRedoServices.PushState();
        _layer.Resize(Width, Height, Align);
        RequestClose?.Invoke();
        _instances.Map.OnModified();
    }
}