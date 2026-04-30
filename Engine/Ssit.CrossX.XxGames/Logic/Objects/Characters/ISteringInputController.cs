using Ssit.CrossX.Input;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ISteeringInputController : ISteeringInput
{
    void SetValue(string id, float value);
    void SetButtonState(string id, ButtonState buttonState);
}