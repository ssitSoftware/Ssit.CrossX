using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ssit.CrossX.UI;

internal class StylesContainer
{
    private class StyleDefinition(Type type, MethodInfo methodInfo)
    {
        public Type Type { get; } = type;
        public MethodInfo MethodInfo { get; } = methodInfo;
    }
    
    private readonly StylesContainer _parent;

    private readonly Dictionary<string, List<StyleDefinition>> _styles = new();

    public StylesContainer(StylesContainer parent = null)
    {
        _parent = parent;
    }

    public void ParseStyles(Type type)
    {
        var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);

        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<StyleAttribute>();
            if (attr is null) continue;

            var parameters = method.GetParameters();
            if(parameters.Length != 1)
                continue;
            
            if(!_styles.TryGetValue(attr.Name, out var list))
            {
                list = new List<StyleDefinition>();
                _styles[attr.Name] = list;
            }
            
            list.Add(new StyleDefinition(parameters[0].ParameterType, method));
        }
    }

    public void ApplyStyles(object obj, string classes)
    {
        var styles = classes.Split(',', ' ');
        foreach (var style in styles)
        {
            if (string.IsNullOrWhiteSpace(style))
                continue;
            
            ApplyStyle(obj, style);
        }
    }

    private void ApplyStyle(object obj, string style)
    {
        _parent?.ApplyStyle(obj, style);
        
        if (!_styles.TryGetValue(style, out var list)) return;

        var type = obj.GetType();
        foreach (var info in list)
        {
            if (!type.IsSubclassOf(info.Type) && type != info.Type)
                continue;
            
            info.MethodInfo.Invoke(null, [obj]);
        }
    }
}