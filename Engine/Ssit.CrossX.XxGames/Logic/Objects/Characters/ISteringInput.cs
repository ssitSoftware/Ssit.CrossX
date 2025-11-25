using Ssit.CrossX.Input;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public interface ISteringInput
{
    float HorizontalMove { get; }
    float Vertical { get; }
    ButtonState ClimbUp { get; }
    
    ButtonState Jump { get; }
    ButtonState Attack { get; }
    ButtonState SpecialAttack { get; }
    ButtonState Operate { get; }
    ButtonState Walk { get; }
}