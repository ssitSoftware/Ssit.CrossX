using System.Numerics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface IPendulumSwinger
{
    Vector2 Position { get; }
    void OnAttachPosition(Vector2 position);
}
