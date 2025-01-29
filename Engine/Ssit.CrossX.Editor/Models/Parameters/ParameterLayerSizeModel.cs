using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Ssit.CrossX.Games.Map;

namespace Ssit.CrtossX.Editor.Models.Parameters;

public class ParameterLayerSizeModel : ParameterModel
{
    private readonly Func<Task> _resizeLayerAction;
    private string _info;

    public MapLayer Layer { get; }
    public ICommand ResizeLayerCommand { get; }
    
    public string Info
    {
        get => _info;
        private set => SetField(ref _info, value);
    }

    public ParameterLayerSizeModel(string name, object layer, Func<Task> resizeLayerAction)
        : base(name)
    {
        _resizeLayerAction = resizeLayerAction;
        ResizeLayerCommand = new AsyncRelayCommand(ResizeLayer);
        Layer = (MapLayer)layer;
        UpdateInfo();
    }

    private async Task ResizeLayer()
    {
        await _resizeLayerAction.Invoke();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        Info = $"{Layer.Width} x {Layer.Height}";
    }
}