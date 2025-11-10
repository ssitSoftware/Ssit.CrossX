using System.Collections.Generic;
using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer.Objects;

public class ObjectAndEnvirionment<TObject> where TObject: IBodyOwner
{
    public bool IsOnGround { get; private set; }
    public bool WasOnGround { get; private set; }

    private TObject _object;

    private readonly List<ICollider> _colliders = new List<ICollider>();

    public IReadOnlyList<ICollider> LastGroundCollisions => _colliders;

    public ObjectAndEnvirionment(TObject @object)
    {
        _object = @object;
        @object.Body.Updated += Body_Updated;
    }

    public void Recalculate()
    {
        WasOnGround = IsOnGround;

        var aabb = _object.Body.Colliders[0].Aabb;

        var minTop = aabb.Bottom;
            
        aabb.Top = aabb.Bottom - 0.01f;
        aabb.Bottom += 0.1f;
        aabb.Left += 0.01f;
        aabb.Right -= 0.01f;

        _colliders.Clear();
        if (_object.Body.Simulation.CheckCollision(aabb, _object.Body, 0, _colliders, ColliderType.Static | ColliderType.Dynamic))
        {
            for (var idx = 0; idx < _colliders.Count;)
            {
                if ((_colliders[idx].Material.ColliderGroup & _object.Body.Colliders[0].Material.ColliderGroup) == 0)
                {
                    _colliders.RemoveAt(idx);
                    continue;
                }

                if(_colliders[idx].Aabb.Top < minTop)
                {
                    _colliders.RemoveAt(idx);
                    continue;
                }

                ++idx;
            }
        }
        IsOnGround = _colliders.Count > 0;
    }

    private void Body_Updated()
    {
        Recalculate();
    }
}