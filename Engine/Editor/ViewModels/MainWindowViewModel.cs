using Ssit.CrossX.IoC;

namespace Editor.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public EditorViewModel Editor { get; }
    
    public MainWindowViewModel(IIoCContainer container)
    {
        Editor = container.IoCConstruct<EditorViewModel>();
    }
}