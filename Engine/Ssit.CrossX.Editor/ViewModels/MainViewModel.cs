using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Ssit.CrtossX.Editor.Helpers;
using Ssit.CrtossX.Editor.Models;
using Ssit.CrtossX.Editor.Service;
using Ssit.CrtossX.Editor.Tools;
using CommunityToolkit.Mvvm.Input;
using Ssit.CrossX.Games.Map;

namespace Ssit.CrtossX.Editor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IServices _services;
        private readonly IEditorInstances _instances;
        private readonly IFileService _fileService;
        private readonly IWindowService _windowService;
        private bool _isModified;
        
        public EditorViewModel EditorViewModel { get; }
        public TilesetSelectorViewModel TilesetSelectorViewModel { get; }

        public ImageSelectorViewModel ObjectsSelector { get; }
        public ImageSelectorViewModel ImagesSelector { get; }
        
        public ITilesetsContainer TilesetsContainer { get; }
    
        public MenuItemModel[] Menu { get; }
        public IEditorInstances Instances => _instances;

        public bool IsModified
        {
            get => _isModified;
            set
            {
                if (SetField(ref _isModified, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                    UpdateTitle();
                }
            }
        }

        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }

        public ICommand EscapeCommand { get; }
        public ICommand HotkeyCommand { get; }
        
        public ICommand EnterFullscreenCommand { get; }
        public IAsyncRelayCommand SaveCommand { get; }
        public IAsyncRelayCommand SaveAsCommand { get; }
        public ICommand HorizontalFlipCommand { get; }
        
        public MapFile MapFile
        {
            get => _mapFile;
            private set
            {
                var oldMap = _mapFile;
                
                if (SetField(ref _mapFile, value))
                {
                    EditorViewModel.SelectedLayer =
                        MapFile.Layers.FirstOrDefault( o=> o.Name.ToLowerInvariant() == "main") ??
                        MapFile.Layers.FirstOrDefault(o => o.HorizontalSpeed == 1 && o.VerticalSpeed == 1) ??
                        MapFile.Layers.First();
                    
                    _instances.SetMap(_mapFile);
                    
                    EditorViewModel.Redraw();
                    IsModified = _mapFile.IsModified;

                    if (oldMap is not null)
                    {
                        oldMap.PropertyChanged -= MapOnPropertyChanged;
                    }

                    _mapFile.PropertyChanged += MapOnPropertyChanged;
                    _instances.UndoRedoServices.Clear();
                    UpdateTitle();
                }
            }
        }

        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        private string _filePath;
        private string _title;
        private MapFile _mapFile;

        public MainViewModel(IServices services, 
            IEditorInstances instances, 
            IFileService fileService,
            IObjectsContainer objectsContainer,
            IImagesContainer imagesContainer,
            IWindowService windowService)
        {
            _services = services;
            _instances = instances;
            _fileService = fileService;
            _windowService = windowService;

            ObjectsSelector = _services.Create<ImageSelectorViewModel>(new ImageSelectorViewModel.Parameters
            {
                Categories = objectsContainer.Categories,
                Images = objectsContainer.Objects,
                SelectionChanged = s =>
                {
                    var tool = _instances.Tools.GetTool<InsertObjectTool>();
                    tool.Object = s as EditorObject;
                    _instances.Tools.Current = tool;
                    _instances.Editor.Redraw();
                }
            });
            
            ImagesSelector = _services.Create<ImageSelectorViewModel>(new ImageSelectorViewModel.Parameters
            {
                Categories = imagesContainer.Categories,
                Images = imagesContainer.Images,
                SelectionChanged = s =>
                {
                    var tool = _instances.Tools.GetTool<InsertImageTool>();
                    tool.Image = s;
                    _instances.Tools.Current = tool;
                    _instances.Editor.Redraw();
                }
            });

            NewCommand = new AsyncRelayCommand(New);
            OpenCommand = new AsyncRelayCommand(Open);
            SaveCommand = new AsyncRelayCommand(Save, () => IsModified);
            SaveAsCommand = new AsyncRelayCommand(SaveAs);

            UndoCommand = new RelayCommand(() => _instances.UndoRedoServices.Undo());
            RedoCommand = new RelayCommand(() => _instances.UndoRedoServices.Redo());

            EscapeCommand = new RelayCommand(() =>
            {
                EditorViewModel.IsFullscreen = false;
                EditorViewModel.SelectedTool = SelectionTool.Name;
            });

            HotkeyCommand = new RelayCommand<HotkeyActions>(a => _instances.Tools.Current.OnHotkeyAction(a));
            
            EnterFullscreenCommand = new RelayCommand(() => EditorViewModel.IsFullscreen = true);

            HorizontalFlipCommand = new RelayCommand(() =>
            {
                if (_instances.Tools.Current is InsertImageTool tool)
                {
                    tool.Flipped = !tool.Flipped;
                }
                EditorViewModel.Redraw();
            });

            EditorViewModel = _services.Create<EditorViewModel>();
            
            New();
            
            TilesetSelectorViewModel = _services.Create<TilesetSelectorViewModel>();

            TilesetsContainer = instances.TilesetsContainer;
            
            Menu = GenerateMenu();
        }
        
        private void MapOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsModified = MapFile?.IsModified ?? false;
        }
        
        private void UpdateTitle()
        {
            string append = MapFile.IsModified ? "*" : "";
            var name = _instances.Template.Name;
            if (_filePath == null)
            {
                Title = $"{name} - New Map" + append;
            }
            else
            {
                Title = $"{name} - " + Path.GetFileNameWithoutExtension(_filePath) + append;
            }
        }
    
        private MenuItemModel[] GenerateMenu()
        {
            return new[]
            {
                new MenuItemModel("File", new[]
                {
                    new MenuItemModel("New", NewCommand),
                    new MenuItemModel("Open", OpenCommand),
                    new MenuItemModel("Save", SaveCommand),
                    new MenuItemModel("Save As...", SaveAsCommand),
                }),
                new MenuItemModel("Edit", new[]
                {
                    new MenuItemModel("Undo", null),
                    new MenuItemModel("Redo", null)
                }),
                new MenuItemModel("Window", null)
            };
        }

        private async Task Open()
        {
            try
            {
                var path = await _fileService.ChooseFile(true, "Open Map File", "Map File (.map)");

                if (path == null)
                    return;

                if (await CheckIfSave() == false)
                {
                    return;
                }
                
                using var stream = File.Open(path, FileMode.Open);
                var mapFile = MapFile.FromStream(stream, _instances.Template,
                    _instances.TilesetsContainer.TileSets.Select(o => o.Path).ToArray());

                if (mapFile.TemplateId == _instances.Template.Guid)
                {
                    _filePath = path;
                    MapFile = mapFile;
                    IsModified = false;
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private async Task New()
        {
            if (!await CheckIfSave())
                return;

            var template = _instances.Template;

            var mapFile = new MapFile(template.Guid, template.TileSize, template.TileSets.ToArray());
        
            foreach (var layer in template.Layers)
            {
                mapFile.Layers.Add(new MapLayer(layer.Size.Width, layer.Size.Height)
                {
                    Name = layer.Name,
                    HorizontalSpeed = layer.HorizontalSpeed,
                    VerticalSpeed = layer.VerticalSpeed,
                    Depth = layer.Depth
                });
            }

            MapFile = mapFile;
            IsModified = false;
        }

        private async Task<bool> CheckIfSave()
        {
            if (IsModified)
            {
                var result = await _windowService.ShowMessageBox("Save changes?", "Do you want to save changes?",
                    MessageBoxType.YesNoCancel);

                if (!result.HasValue)
                {
                    return false;
                }

                if (result.Value)
                {
                    await Save();
                    return !IsModified;
                }
            }

            return true;
        }

        private async Task SaveAs()
        {
            try
            {
                var path = await _fileService.ChooseFile(false, "Save Map File", "Map File (.map)");
                _filePath = path;
                await Save();
            }
            catch (OperationCanceledException)
            {

            }
        }
    
        private async Task Save()
        {
            if (_filePath is null)
            {
                await SaveAs();
                return;
            }
        
            using (var stream = File.Open(_filePath, FileMode.Create))
            {
                //using var gzipStream = new GZipStream(stream, CompressionLevel.SmallestSize);
                MapFile.Save(stream, true);
            }
        }

        private bool _forceClose = false;
        public bool CanClose()
        {
            if (_forceClose) return true;
            
            TryClose();
            return false;
        }

        private async void TryClose()
        {
            var canClose = await CheckIfSave();

            if (canClose)
            {
                _forceClose = true;
                _windowService.CloseMainWindow();
            }
        }
    }
}