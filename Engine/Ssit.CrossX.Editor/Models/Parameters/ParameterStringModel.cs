using System.Reflection;
using Ssit.CrossX.Games.Editor;

namespace Ssit.CrtossX.Editor.Models.Parameters;

public class ParameterStringModel : ParameterModel<string>
{
    public ParameterStringModel(string name, object owner, PropertyInfo propertyInfo, IPropertyHandler handler) : base(name, owner, propertyInfo, handler)
    {
    }

    protected override void Validate()
    {
        IsInvalid = string.IsNullOrWhiteSpace(Value);
    }
}