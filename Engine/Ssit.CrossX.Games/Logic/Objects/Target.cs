using System.Numerics;
using Ssit.CrossX.Games.Editor;
using Ssit.CrossX.Games.Logic.Map;

namespace Ssit.CrossX.Games.Logic.Objects;

public class Target : ITarget
{
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