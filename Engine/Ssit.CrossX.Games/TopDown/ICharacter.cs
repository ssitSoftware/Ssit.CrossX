using System.Numerics;

namespace Ssit.CrossX.Games.TopDown;

public interface ICharacter
{
    Vector2 Position { get; }
    float Orientation { get; }
    float Size { get; }
}