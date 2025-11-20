using System;
using System.Collections.Generic;
using Ssit.CrossX.XxGames.Physics.Coliders;

namespace Ssit.CrossX.XxGames.Physics;

public interface ISimulation: IDisposable
{
    SimulationParameters SimulationParameters { get; }
    float MovementEpsilon { get; }

    event Action<IBody> BodyRemoved;
    event Action<IBody> BodyAdded;
    event Action<bool> Activate;

    Aabb Bounds { get; }

    IReadOnlyList<IBody> Bodies { get; }
    void Update(float timeInSeconds, Action<float> onFixedUpdate);
    IBody CreateBody();
    IBody CreateBody(IBodyOwner owner);
    void RemoveBody(IBody body);
    void InitializeStaticColliders(Aabb bounds, IEnumerable<ICollider> colliders);
    bool CheckCollision(Aabb aabb, IBody testingBody, float epsilon = 0, IList<ICollider> colliders = null, ColliderType colliderType = ColliderType.Static | ColliderType.Dynamic);

    void GetColliders(Aabb bounds, IList<ICollider> colliders);
    IReadOnlyList<ICollider> GetColliders(Aabb bounds);
    void Debug_GetQuadTreeAreas(IList<Aabb> aabbs);
    
    ICollider CreateCollider<TCreationParameters>(TCreationParameters creationParameters) where TCreationParameters : ColliderCreationParameters;
}