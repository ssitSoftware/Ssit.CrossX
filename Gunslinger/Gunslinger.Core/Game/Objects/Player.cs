using Ssit.CrossX.Games.Editor;

namespace Gunslinger.Core.Game.Objects;

public class Player
{
    public class Parameters
    {
        [EditorFloat(10,20)]
        public float Speed { get; set; }
    }
}