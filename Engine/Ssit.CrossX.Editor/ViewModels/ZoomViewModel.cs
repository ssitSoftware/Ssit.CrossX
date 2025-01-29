using System;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace Ssit.CrossX.Editor.ViewModels
{
    public class ZoomViewModel: ViewModelBase
    {
        private readonly Action _onZoom;
        public IRelayCommand ZoomOutCommand { get; }
        public IRelayCommand ZoomInCommand { get; }

        public string Info
        {
            get => _info;
            set => SetField(ref _info, value);
        }

        public float Value => (float)_zoom;
    
        private decimal _zoom = 1m;
        private string _info;

        public ZoomViewModel(Action onZoom)
        {
            _onZoom = onZoom;
        
            ZoomInCommand = new RelayCommand(ZoomIn, () => _zoom < 8);
            ZoomOutCommand = new RelayCommand(ZoomOut, () => _zoom > 0.25m);
        
            UpdateZoom();
        }

        public void SetZoom(decimal zoom)
        {
            _zoom = zoom;
            ZoomInCommand.NotifyCanExecuteChanged();
            ZoomOutCommand.NotifyCanExecuteChanged();
            UpdateZoom();
            _onZoom?.Invoke();
        }

        private void UpdateZoom()
        {
            Info = $"{(int) (_zoom * 100)}%";
        }

        private void ZoomOut()
        {
            if (_zoom <= 1)
            {
                _zoom -= 0.25m;
            }
            else
            {
                _zoom -= 1;
            }
        
            if (_zoom < 0.25m)
            {
                _zoom = 0.25m;
            }
        
            ZoomInCommand.NotifyCanExecuteChanged();
            ZoomOutCommand.NotifyCanExecuteChanged();
            UpdateZoom();
            _onZoom?.Invoke();
        }

        private void ZoomIn()
        {
            if (_zoom < 1)
            {
                _zoom += 0.25m;
            }
            else
            {
                _zoom += 1;
            }

            if (_zoom > 8)
            {
                _zoom = 8;
            }
        
            ZoomInCommand.NotifyCanExecuteChanged();
            ZoomOutCommand.NotifyCanExecuteChanged();
            UpdateZoom();
            _onZoom?.Invoke();
        }
    }
}