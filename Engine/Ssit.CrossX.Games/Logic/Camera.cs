using System;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Games.Utils;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.Games.Logic;

internal class Camera(IGameTemplate template, IInputMappings inputMappings): ICamera
{
    private Vector2 _lookAt;

    private Body _primaryTarget;
    private Vector2 _primaryOffset;
    private float _primaryFollowFactor;
    
    private Body _temporaryTarget;
    private Vector2 _temporaryOffset;
    private float _temporaryReturnTime;
    private float _temporaryFollowFactor;
    
    private Body Body => _temporaryTarget ?? _primaryTarget;
    private Vector2 Offset => _temporaryTarget != null ? _temporaryOffset : _primaryOffset + _cameraMove * 6;
    private float FollowFactor => _temporaryTarget != null ? _temporaryFollowFactor : _primaryFollowFactor;
    
    private Action _onTemporaryTargetFocused;

    public Vector2 LookAt => _lookAt.TrimVectorToPixels(template.TrimToPixels);

    private Vector2 _cameraMove;
    
    public void SetPrimaryTarget(Body body, Vector2 offset, float followFactor)
    {
        _primaryTarget = body;
        _primaryOffset = offset;
        _primaryFollowFactor = followFactor;
        
        _lookAt = _primaryTarget.Position + _primaryOffset;
    }

    public void SetTemporaryTarget(Body body, Vector2 offset, float followFactor, Action onFocused, TimeSpan returnAfter)
    {
        _temporaryTarget = body;
        _temporaryOffset = offset;
        _temporaryReturnTime = (float)returnAfter.TotalSeconds;
        _onTemporaryTargetFocused = onFocused;
        _temporaryFollowFactor = followFactor;
    }

    public void Update(float dt)
    {
        if (Body is null)
            return;

        var moveX = inputMappings[0].GetAxis("CameraX");
        var moveY = inputMappings[0].GetAxis("CameraY");

        var dir = new Vector2(moveX, moveY);
        _cameraMove = dir; 
        // var factor = MathF.Min(1, dt * MathF.Max(0, 1- dir.Length()) * 10);
        //
        // _cameraMove = (1-factor) * _cameraMove + Vector2.Zero * factor;
        // _cameraMove += dir * dt * 6;
        //
        // var len = MathF.Min(_cameraMove.Length(), 1);
        // if (len > 0)
        // {
        //     _cameraMove = Vector2.Normalize(_cameraMove) * len;
        // }

        var target = Body.Position + Offset;

        if (_primaryFollowFactor > 100000)
        {
            _lookAt = target;
            return;
        }
        
        var factor = MathF.Min(1, dt * FollowFactor);
        
        var newLookAt = factor * target + (1 - factor) * _lookAt;
        var diff = newLookAt - target;
        
        var epsilon = 0.125f / template.TileSize;
         if (MathF.Abs(diff.X) < epsilon && MathF.Abs(diff.Y) < epsilon)
        {
            newLookAt = target;
        }

        _lookAt = newLookAt;
        
        if (_temporaryTarget != null && (_lookAt - target).Length() < 0.5f)
        {
            _onTemporaryTargetFocused?.Invoke();
            _onTemporaryTargetFocused = null;
            
            _temporaryReturnTime -= dt;
            if (_temporaryReturnTime <= 0)
            {
                _temporaryTarget = null;
                _temporaryOffset = Vector2.Zero;
                _temporaryReturnTime = 0f;
            }
        }
    }
}