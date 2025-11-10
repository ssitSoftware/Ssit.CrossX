// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Ssit.CrossX.XxGames.PhysicsCore;
// using Ssit.CrossX.XxGames.PhysicsCore.Coliders;
//
// namespace Ssit.CrossX.XxGames.Platformer.Objects;
//
// public class KinematicObject : GameObject, IVisualStateSource
// {
//     public class Parameters
//     {
//         public IObjectSource<ISwitch> Switch { get; set; }
//         public IObjectSource<ITarget> Target { get; set; }
//     }
//
//     private ITarget target;
//     private ISwitch @switch;
//
//     public event Action VisualStateChanged;
//     public string VisualState => "Off";
//
//     public VectorF VisualTransform => new VectorF(1, 1);
//
//     public KinematicObject(ISimulation simulation, Parameters parameters, IEnumerable<ColliderCreationParameters> colliders, CreateObjectParameters createObjectParameters) : base(simulation)
//     {
//         var bodyColliders = colliders.Select(o =>
//         {
//             o.AttachToBody = Body;
//             return simulation.CreateCollider(o);
//         }).ToArray();
//
//         Body.AddColliders(bodyColliders);
//         Body.Mass = 10000;
//         Body.IsKinematic = true;
//         Body.Position = createObjectParameters.Position;
//
//         parameters.Target.OnObjectResolved(obj => target = obj);
//         parameters.Switch.OnObjectResolved(obj => @switch = obj);
//
//         parameters.Target.OnAllResolved(GeneratePath);
//     }
//
//     private void GeneratePath()
//     {
//             
//     }
//
//     public void RaiseVisualEvent(string name, IParameter[] parameters) {}
// }