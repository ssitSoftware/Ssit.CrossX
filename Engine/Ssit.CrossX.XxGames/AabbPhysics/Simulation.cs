using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.XxGames.Algorithms;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Coliders;

namespace Ssit.CrossX.XxGames.AabbPhysics;

internal class Simulation : ISimulation
{
    private double _timeToUpdate = 0;

    public float MovementEpsilon => MovementCollisionCalculator.MovementEpsilon;

    public Vector2 GravityAcceleration { get; set; } = new(0, 10);

    public SimulationParameters SimulationParameters { get; set; } = new()
    {
        GravityAcceleration = new Vector2(0, 10),
        MaxHorizontalSpeed = 1000,
        MaxVerticalSpeed = 1000,
        TimeDelta = 0.01f
    };

    public Aabb Bounds => _collidersRootNode.Aabb;

    public event Action<IBody> BodyRemoved;
    public event Action<IBody> BodyAdded;

    private readonly List<Body> _bodies = new();
    private readonly List<Body> _detachedBodies = new();

    public event Action<bool> Activate;
    private QuadTreeNode<ICollider> _collidersRootNode;
    private readonly List<ICollider> _collidersToTest = new();

    public IReadOnlyList<IBody> Bodies => _bodies;

    public ICollider CreateCollider<TCreationParameters>(TCreationParameters creationParameters) where TCreationParameters: ColliderCreationParameters
        => CollidersFactory.Create(creationParameters);

    public void GetColliders(Aabb bounds, IList<ICollider> colliders) => _collidersRootNode.GetElements(bounds, colliders);

    public IBody CreateBody()
    {
        var body = new Body(this);
        _bodies.Add(body);
        return body;
    }

    public IBody CreateBody(IBodyOwner owner)
    {
        var body = new Body(this, owner);
        _bodies.Add(body);
        return body;
    }

    public void AttachBody(Body body)
    {
        _detachedBodies.Remove(body);
        if (_bodies.Contains(body)) return;
        _bodies.Add(body);
    }

    public void DetachBody(Body body, bool keepForLater)
    {
        _bodies.Remove(body);
        if(keepForLater)
        {
            _detachedBodies.Add(body);
        }

        foreach (var collider in body.Colliders)
        {
            _collidersRootNode.RemoveElement(collider);
        }
    }

    public void SortBodies()
    {
        _bodies.Sort((o1, o2) => o1.UpdateOrder - o2.UpdateOrder);
    }

    public bool CheckCollision(Aabb aabb, IBody testingBody, double epsilon = 0, IList<ICollider> colliders = null, ColliderType colliderType = ColliderType.Static | ColliderType.Dynamic)
    {
        _collidersToTest.Clear();
        _collidersRootNode.GetElements(aabb, _collidersToTest);

        for (var idx = 0; idx < _collidersToTest.Count; ++idx)
        {
            var collider = _collidersToTest[idx];

            if (!collider.IsActive) continue;
            if (collider.AttachedBody == testingBody) continue;
            if (!colliderType.HasFlag(collider.Type)) continue;

            var colliderAabb = collider.Aabb;

            if (!colliderAabb.Intersects(aabb, epsilon)) continue;

            if (colliders == null) return true;
            colliders.Add(collider);
        }

        return colliders?.Count > 0;
    }

    public void UpdateColliderInTree(ICollider collider)
    {
        _collidersRootNode.RemoveElement(collider);
        if (collider.Type.HasFlag(ColliderType.Particle)) return;
        _collidersRootNode.AddElement(collider);
    }

    public void InitializeStaticColliders(Aabb bounds, IEnumerable<ICollider> colliders)
    {
        var maxElementsPerNode = 8;
        _collidersRootNode = new QuadTreeNode<ICollider>(bounds, colliders, maxElementsPerNode);
    }

    public void Update(double timeInSeconds)
    {
        SortBodies();
        OnUpdate(timeInSeconds);

        _timeToUpdate += timeInSeconds;

        while (_timeToUpdate >= SimulationParameters.TimeDelta)
        {
            OnFixedUpdate();
            _timeToUpdate -= SimulationParameters.TimeDelta;
        }
    }

    protected virtual void OnUpdate(double time)
    {
        for (var idx = 0; idx < _bodies.Count; ++idx)
        {
            _bodies[idx].Update(time);
        }
    }

    protected virtual void OnFixedUpdate()
    {
        for (var idx = 0; idx < _bodies.Count; ++idx)
        {
            _bodies[idx].FixedUpdate();
        }

        for (var idx = 0; idx < _bodies.Count; ++idx)
        {
            _bodies[idx].PostFixedUpdate();
        }
    }

    public void Dispose()
    {
        var objects = _bodies.ToList();
        foreach (var obj in objects)
        {
            obj.Dispose();
        }

        objects = _detachedBodies.ToList();
        foreach (var obj in objects)
        {
            obj.Dispose();
        }
        _bodies.Clear();
        _detachedBodies.Clear();
    }
}