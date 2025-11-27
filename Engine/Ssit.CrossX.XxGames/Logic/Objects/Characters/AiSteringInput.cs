using Ssit.CrossX.Input;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public class AiSteringInput : ISteringInput
{
    public float HorizontalMove { get; set; }
    public float Vertical { get; set; }
    public ButtonState Jump { get; set; }
    public ButtonState Attack { get; set; }
    public ButtonState SpecialAttack { get; set; }
    public ButtonState Operate { get; set; }
    public ButtonState Walk { get; set; }
}