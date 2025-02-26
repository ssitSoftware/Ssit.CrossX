using System.Diagnostics.CodeAnalysis;
using Ssit.CrossX.Games.Editor;

namespace Ssit.CrossX.Games.Map;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class StaticObjectParameters
{
    [EditorInt(0, 5000, 10)]
    public int AnimationTimeOffsetInMs { get; set; }
}