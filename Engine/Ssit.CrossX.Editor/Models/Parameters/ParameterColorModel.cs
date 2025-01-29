using System.Reflection;
using Breeze.Engine;
using Ssit.CrossX.Games.Editor;

namespace Ssit.CrtossX.Editor.Models.Parameters;

public class ParameterColorModel: ParameterModel<RgbaColor>
{
    public ParameterColorModel(string name, object owner, PropertyInfo propertyInfo, IPropertyHandler handler) : base(name, owner, propertyInfo, handler)
    {
    }
}