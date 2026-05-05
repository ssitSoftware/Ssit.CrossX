using Ssit.CrossX.Input;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ISteeringInput
{
    ButtonState Button(string id);
    float Value(string id);
}