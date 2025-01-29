using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Ssit.CrtossX.Editor.Models.Parameters;

namespace Ssit.CrtossX.Editor.Controls;

public class PropertiesTemplateSelector: IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> Templates {get;} = new();
    
    public Control Build(object data)
    {
        return Templates[data.GetType().Name].Build(data);
    }

    public bool Match(object data)
    {
        return data is ParameterModel;
    }
}