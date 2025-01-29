using Ssit.CrossX.Editor.Service;

namespace Ssit.CrossX.Editor.Tools;

public class EmptyTool : EditorTool
{
    public new const string Name = "Empty";
    public EmptyTool(IEditorInstances instances) : base(Name, instances)
    {
    }
}