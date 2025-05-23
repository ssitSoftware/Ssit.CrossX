using Ssit.CrossX.Input;

namespace Nokemono.Core;

internal static class GameControls
{
    public const string Horizontal = nameof(Horizontal);
    public const string Vertical = nameof(Vertical);
    public const string Jump = nameof(Jump);
    public const string Melee = nameof(Melee);
    public const string Special = nameof(Special);
    public const string Operate = nameof(Operate);

    public static void RegisterGameControls(IInputMappings mappings)
    {
        mappings.Mapper(0)
            .Clear()
            .MapButton(Jump, Key.C)
            .MapButton(Melee, Key.X)
            .MapButton(Special, Key.Z)
            .MapButton(Operate, Key.Z)
            .MapButton(Jump, GameControllerButton.A)
            .MapButton(Melee, GameControllerButton.X)
            .MapButton(Special, GameControllerButton.B)
            .MapButton(Operate, GameControllerButton.B)
            .MapAxis(Horizontal, GameControllerButton.DPadLeft, GameControllerButton.DPadRight)
            .MapAxis(Vertical, GameControllerButton.DPadUp, GameControllerButton.DPadDown)
            .MapAxis(Horizontal, GameControllerAxis.LeftX)
            .MapAxis(Vertical, GameControllerAxis.LeftY)
            .MapAxis(Horizontal, Key.Left, Key.Right)
            .MapAxis(Vertical, Key.Up, Key.Down);
    }
}