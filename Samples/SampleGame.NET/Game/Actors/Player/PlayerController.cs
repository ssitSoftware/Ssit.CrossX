using System.Numerics;
using SampleGame.Game.Logic;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Actors.Player;

public class PlayerController(IInputMappings inputMappings) : IGameObjectController
{
    public Vector2 GetMoveDirection()
    {
        var mx = inputMappings[0].GetAxis("Horizontal");
        var my = inputMappings[0].GetAxis("Vertical");
        return new Vector2(mx, my);
    }

    public Vector2 GetAimDirection()
    {
        var mx = inputMappings[0].GetAxis("AimX");
        var my = inputMappings[0].GetAxis("AimY");
        return new Vector2(mx, my);
    }

    public ButtonState ShootButton => inputMappings[0].GetButton("Shoot");
    public ButtonState MeleeButton => inputMappings[0].GetButton("Melee");
    public ButtonState ReloadButton => inputMappings[0].GetButton("Reload");
    public ButtonState RollButton => inputMappings[0].GetButton("Roll");
}