using Ssit.CrtossX.Editor.Service;

namespace Ssit.CrtossX.Editor.Tools;

public class EmptyTool : EditorTool
{
    public new const string Name = "Empty";
    public EmptyTool(IEditorInstances instances) : base(Name, instances)
    {
    }
        
}