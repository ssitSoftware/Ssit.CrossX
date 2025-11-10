// using System;
// using System.Numerics;
// using Ssit.CrossX.XxGames.PhysicsCore;
// using XxGames.Platformer;
// using XxGames.Platformer.Objects;
//
// namespace Ssit.CrossX.XxGames.Platformer.Objects;
//
// public abstract class GameObject : IBodyOwner, IHittable
// {
//     public IBody Body { get; }
//
//     public event Action FixedUpdate;
//
//     protected GameObject(ISimulation simulation)
//     {
//         Body = simulation.CreateBody(this);
//     }
//
//     public virtual void Dispose()
//     {
//             
//     }
//
//     public virtual bool Hit(HitKind kind, ICollider collider, Vector2 impact)
//     {
//         return false;
//     }
//
//     public virtual void OnFixedUpdate(out bool cancelUpdate)
//     {
//         cancelUpdate = false;
//         FixedUpdate?.Invoke();
//     }
//
//     public virtual void OnPostFixedUpdate()
//     {
//             
//     }
//
//     public virtual void OnUpdate(double time)
//     {
//             
//     }
//
//     public virtual void Start()
//     {
//             
//     }
// }