using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.XxFormats.Editor;
using Ssit.CrossX.XxGames.Platformer.Builders;

namespace Ssit.CrossX.Games.Logic.Objects;

public class Target : ITarget
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Parameters
    {
        [EditorLink(typeof(ITarget))] public int Next { get; set; }
    }

    public Vector2 Position { get; }
    public ITarget Next { get; private set; }

    public Target(ObjectCreationParameters<Parameters> parameters)
    {
        Position = parameters.Position;
        parameters.LinkMap.RequestLink<ITarget>(parameters.Parameters.Next, t => Next = t);
    }
}