using System.Reflection;
using Ssit.CrossX.XxFormats.Editor;

namespace Ssit.CrossX.Editor.Models.Parameters;

public class ParameterBooleanModel: ParameterModel<bool>
{
    public ParameterBooleanModel(string name, object owner, PropertyInfo propertyInfo, IPropertyHandler handler) 
        : base(name, owner, propertyInfo, handler)
    {
    }
}