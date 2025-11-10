// using System;
// using System.Collections.Generic;
// using System.Linq;
// using EbatianoSoftware.CrossX.Primitives;
// using Ssit.CrossX.XxGames.Platformer.Objects;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.PhysicsCore;
// using XxGames.PhysicsCore.Colliders;
//
// namespace XxGames.Platformer.Objects;
//
// [CreationParametersType(typeof(Parameters))]
// public class Switch : GameObject, ISwitch, IVisualStateSource
// {
//     public enum SwitchMode
//     {
//         None,
//         OnDemand,
//         Hit,
//         HitLeft,
//         HitRight
//     }
//
//     public class Parameters
//     {
//         public bool IsOn { get; set; }
//         public SwitchMode SwitchOn { get; set; }
//         public SwitchMode SwitchOff { get; set; }
//     }
//
//     public bool IsOn { get; private set; }
//
//     public string VisualState { get; private set; }
//
//     public VectorF VisualTransform => new VectorF(1, 1);
//
//     public event Action VisualStateChanged;
//
//     public Switch(ISimulation simulation, Parameters parameters, IEnumerable<ColliderCreationParameters> colliders, CreateObjectParameters createObjectParameters) : base(simulation)
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
//         IsOn = parameters.IsOn;
//
//         VisualState = IsOn ? "On" : "Off";
//     }
//     public void RaiseVisualEvent(string name, IParameter[] parameters) { }
// }