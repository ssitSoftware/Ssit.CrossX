using System.Reflection;
using Ssit.CrossX.Games.Editor;

namespace Ssit.CrtossX.Editor.Models.Parameters;

public class ParameterBooleanModel: ParameterModel<bool>
{
    public ParameterBooleanModel(string name, object owner, PropertyInfo propertyInfo, IPropertyHandler handler) 
        : base(name, owner, propertyInfo, handler)
    {
    }
}