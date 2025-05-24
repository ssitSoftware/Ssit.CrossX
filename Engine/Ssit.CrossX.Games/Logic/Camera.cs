using System;
using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Games.Utils;

namespace Ssit.CrossX.Games.Logic;

internal class Camera(IGameTemplate template): ICamera
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

    public Vector2 LookAt => _lookAt.TrimVectorToPixels(template.TrimToPixels);
    
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

        if (_primaryFollowFactor > 100000)
        {
            _lookAt = target;
            return;       
        }
        
        var factor = dt * FollowFactor;
        
        var dist = (_lookAt - target).Length();
        dist = MathF.Max((2-dist), 1);

        factor = MathF.Min(1, factor * dist);
        
        var newLookAt = factor * target + (1 - factor) * _lookAt;
        var diff = newLookAt - target;
        
        var epsilon = 0.25f / template.TileSize;
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