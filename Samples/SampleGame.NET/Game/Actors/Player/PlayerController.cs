using System;
using System.Numerics;
using SampleGame.Game.Logic;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Input;

namespace SampleGame.Game.Actors.Player;

public class PlayerController(IInputMappings inputMappings, IPointingDevices pointingDevices, IRenderingWindow window, IKeyboard keyboard) : IGameObjectController
{
    private Vector2? _lastHoverPosition;
    private DateTime _lastMouseInteractionTime;
    
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
        
        if(mx != 0 || my != 0) 
            return new Vector2(mx, my);

        return GetMouseAim();
    }

    private Vector2 GetMouseAim()
    {
        if (!pointingDevices.HoverPosition.HasValue)
        {
            _lastHoverPosition = null;
            return Vector2.Zero;
        }

        if (_lastHoverPosition.HasValue)
        {
            var offset = _lastHoverPosition.Value - pointingDevices.HoverPosition.Value;
            if (offset != Vector2.Zero)
            {
                _lastMouseInteractionTime = DateTime.Now;
            }
        }
        _lastHoverPosition = pointingDevices.HoverPosition.Value;

        if (keyboard.GetKey(Key.MouseLeft).IsDown ||
            keyboard.GetKey(Key.MouseRight).IsDown)
        {
            _lastMouseInteractionTime = DateTime.Now;
        }

        if ((DateTime.Now - _lastMouseInteractionTime).TotalSeconds > 2)
        {
            return Vector2.Zero;
        }
        
        var pos = pointingDevices.HoverPosition.Value;
        
        var dir = pos - window.Size.ToVector() / 2;
        dir = Vector2.Normalize(dir);
        return dir;
    }

    public ButtonState ShootButton => inputMappings[0].GetButton("Shoot");
    public ButtonState MeleeButton => inputMappings[0].GetButton("Melee");
    public ButtonState ReloadButton => inputMappings[0].GetButton("Reload");
    public ButtonState RollButton => inputMappings[0].GetButton("Roll");
}