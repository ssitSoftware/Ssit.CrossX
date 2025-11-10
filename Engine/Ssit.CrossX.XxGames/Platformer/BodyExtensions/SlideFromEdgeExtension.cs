// MIT License - Copyright © ebatianoSoftware
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.BodyExtensions;

public class SlideFromEdgeExtension: IBodyExtension
{
    private readonly IBody _body;
    private readonly List<ICollider> _collidersBuffer = new List<ICollider>();

    private readonly float _slideSpeed = 0;
    private readonly float _stableWidthNormalized = 0;

    public SlideFromEdgeExtension(IBody body, float stableWidthNormalized, float slideSpeed)
    {
        _body = body;
        _body.Updated += OnBodyUpdated;
        _slideSpeed = slideSpeed;
        _stableWidthNormalized = stableWidthNormalized;
    }

    private void OnBodyUpdated()
    {
        foreach (var collider in _body.Colliders)
        {
            var aabb = collider.Aabb;
            _collidersBuffer.Clear();

            if (!_body.Simulation.CheckCollision(new Aabb(aabb.Left, aabb.Bottom - 0.02f, aabb.Right, aabb.Bottom + 0.05f), _body, 0, _collidersBuffer)) return;

            for (var idx = 0; idx < _collidersBuffer.Count; )
            {
                if(!_collidersBuffer[idx].Material.Sides.HasFlag(ColliderSides.Top))
                {
                    _collidersBuffer.RemoveAt(idx);
                    continue;
                }

                if (_collidersBuffer[idx].Aabb.Top < aabb.Bottom)
                {
                    _collidersBuffer.RemoveAt(idx);
                    continue;
                }

                idx++;
            }

            if (_collidersBuffer.Count == 0) return;

            var slideLeft = aabb.Width / 2;
            var slideRight = aabb.Width / 2;

            for (var idx = 0; idx < _collidersBuffer.Count; ++idx)
            {
                var colliderAabb = _collidersBuffer[idx].Aabb;
                slideLeft = MathF.Min(slideLeft, MathF.Max(0, colliderAabb.Left - aabb.Left - aabb.Width * _stableWidthNormalized));
                slideRight = MathF.Min(slideRight, MathF.Max(0, aabb.Right - aabb.Width * _stableWidthNormalized - colliderAabb.Right));
            }

            if (slideLeft > 0)
            {
                slideLeft /= aabb.Width * _stableWidthNormalized;
                _body.Velocity -= new Vector2(_body.Simulation.SimulationParameters.TimeDelta * _slideSpeed * MathF.Pow(slideLeft, 0.25f), 0);
                _body.Touch();
            }
            else if (slideRight > 0)
            {
                slideRight /= aabb.Width * _stableWidthNormalized;
                _body.Velocity += new Vector2(_body.Simulation.SimulationParameters.TimeDelta * _slideSpeed * MathF.Pow(slideRight, 0.25f), 0);
                _body.Touch();
            }
        }
    }

    public void Dispose()
    {
        _body.Updated -= OnBodyUpdated;
    }
}