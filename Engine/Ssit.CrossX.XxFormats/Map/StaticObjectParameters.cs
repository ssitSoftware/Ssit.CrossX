using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.XxFormats.Editor;

namespace Ssit.CrossX.XxFormats.Map;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class StaticObjectParameters
{
    [EditorInt(0, 5000, 10)]
    public int AnimationTimeOffsetInMs { get; set; }
}