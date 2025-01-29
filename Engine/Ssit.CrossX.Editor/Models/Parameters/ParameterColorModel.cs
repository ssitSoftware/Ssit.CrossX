using System.Reflection;
using Ssit.CrossX.Games.Editor;

namespace Ssit.CrossX.Editor.Models.Parameters;

public class ParameterColorModel: ParameterModel<RgbaColor>
{
    public ParameterColorModel(string name, object owner, PropertyInfo propertyInfo, IPropertyHandler handler) : base(name, owner, propertyInfo, handler)
    {
    }
}