// MIT License - Copyright © ebatianoSoftware
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.BodyExtensions;

public class StandOnBodyExtension: IBodyExtension
{
    private readonly List<ICollider> _hookedColliders = new();

    private readonly IBody _body;
    public StandOnBodyExtension(IBody body)
    {
        _body = body;

        _body.Colliders[0].CollisionWith += AabbCollider_CollisionWith;
        _body.Moved += OnMove;
        _body.Updated += UpdateHookedColliders;
        _body.UpdateOrder = int.MaxValue;
    }

    private void AabbCollider_CollisionWith(bool byMyMovement, ICollider obj, Vector2 impact)
    {
        if (obj.AttachedBody == null) return;
        if (obj.Aabb.Top >= _body.Colliders[0].Aabb.Top) return;

        if (obj.Aabb.Bottom - _body.Simulation.MovementEpsilon < _body.Colliders[0].Aabb.Top)
        {
            if(!_hookedColliders.Contains(obj)) _hookedColliders.Add(obj);
        }
    }

    private void OnMove(Vector2 hookedMove)
    {
        if (hookedMove.Y < 0) hookedMove.Y = 0;

        for (var idx = 0; idx < _hookedColliders.Count; ++idx)
        {
            var body = _hookedColliders[idx].AttachedBody;
            body.Move(hookedMove);
            body.KinematicVelocity = hookedMove / _body.Simulation.SimulationParameters.TimeDelta;
        }

        UpdateHookedColliders();
    }

    private void UpdateHookedColliders()
    {
        var inflatedAabb = _body.Colliders[0].Aabb;
        inflatedAabb.Inflate(0.0001f);

        for (var idx = 0; idx < _hookedColliders.Count;)
        {
            if (!_hookedColliders[idx].Aabb.Intersects(inflatedAabb))
            {
                _hookedColliders[idx].AttachedBody.KinematicVelocity = Vector2.Zero;
                _hookedColliders.RemoveAt(idx);
                continue;
            }

            idx++;
        }
    }

    public void Dispose()
    {
        _body.Colliders[0].CollisionWith -= AabbCollider_CollisionWith;
        _body.Moved -= OnMove;
        _body.Updated -= UpdateHookedColliders;
    }
}