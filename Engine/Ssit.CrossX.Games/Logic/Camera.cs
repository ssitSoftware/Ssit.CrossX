using System;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic;

internal class Camera: ICamera
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
    private Vector2 Offset => _temporaryTarget != null ? _temporaryOffset :_primaryOffset;
    private float FollowFactor => _temporaryTarget != null ? _temporaryFollowFactor : _primaryFollowFactor;
    
    private Action _onTemporaryTargetFocused;

    public Vector2 LookAt => _lookAt;

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
        
        var target = Body.Position + Offset;
        var factor = MathF.Min(1, dt * FollowFactor);
        
        _lookAt = factor * target + (1 - factor) * _lookAt;
        _lookAt = factor * target + (1 - factor) * _lookAt;

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