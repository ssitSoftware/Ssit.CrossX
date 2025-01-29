using System.ComponentModel;
using Ssit.CrtossX.Editor.Tools;

namespace Ssit.CrtossX.Editor.Service
{
    public interface IEditorTools: INotifyPropertyChanged
    {
        EditorTool Current { get; set; }
        EditorTool GetTool(string tool);
        T GetTool<T>() where T : EditorTool;
    }
}