using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ICharacter
{
    bool FaceLeft { get; set; }
    IBody Body { get; }
}