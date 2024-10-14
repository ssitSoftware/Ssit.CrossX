namespace Ssit.Pixel.Framework.Input.Internal;

public abstract class GameControllersBase: IGameControllers
{
    public byte VibrationForce { get; set; } = 3;
    
    public ButtonState GetButton(int player, GameControllerButton button) => GetButtonInternal(player, button);
    public float GetAxis(int player, GameControllerAxis axis) => GetAxisInternal(player, axis);

    public void Vibrate(int player, Vibration low, Vibration high, uint ms)
    {
        if (VibrationForce > 0)
        {
            VibrateInternal(player, (ushort) ((int) low * 0xffff * VibrationForce / 40), (ushort) ((int) high * 0xffff * VibrationForce / 40), ms);
        }
    }

    protected abstract ButtonState GetButtonInternal(int player, GameControllerButton button);
    protected abstract float GetAxisInternal(int player, GameControllerAxis axis);
    protected abstract void VibrateInternal(int player, ushort low, ushort high, uint ms);
}