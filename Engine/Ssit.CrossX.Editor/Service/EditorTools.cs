using System.Collections.Generic;
using System.Linq;
using Ssit.CrtossX.Editor.Tools;
using Breeze.Engine;
using Ssit.CrossX.Games;

namespace Ssit.CrtossX.Editor.Service;

public class EditorTools: BindableModel, IEditorTools
{
    private readonly IEditorInstances _instances;

    public EditorTool Current
    {
        get => _current;
        set
        {
            var old = _current;
            if (SetField(ref _current, value))
            {
                old?.OnFinished();
                _instances.Editor?.Redraw();
            }
        }
    }

    private readonly List<EditorTool> _tools = new();
    private EditorTool _current;

    public EditorTools(IEditorInstances instances, IServices services)
    {
        _instances = instances;
        
        _tools.Add(services.Create<EmptyTool>());
        _tools.Add(services.Create<SelectionTool>());
        _tools.Add(services.Create<InsertTilesTool>());
        _tools.Add(services.Create<EraserTool>());
        _tools.Add(services.Create<InsertObjectTool>());
        _tools.Add(services.Create<InsertImageTool>());
        _tools.Add(services.Create<SetMaterialTool>());
    }

    public EditorTool GetTool(string tool) => _tools.FirstOrDefault( o=>o.Name == tool);

    public T GetTool<T>() where T: EditorTool
    {
        return (T)_tools.First( o => o.GetType() == typeof(T));
    }
}