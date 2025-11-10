// using EbatianoSoftware.CrossX.IoC;
// using EbatianoSoftware.CrossX.Primitives;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using Ssit.CrossX.XxGames.Platformer;
// using Ssit.CrossX.XxGames.Platformer.Objects;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Logic.Values;
// using XxGames.PhysicsCore;
// using XxGames.PhysicsCore.Colliders;
// using XxGames.Platformer.Behaviors.PlayableCharacter;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Objects;
//
// [BehaviorType(typeof(PlayableCharacterBehavior))]
// public class PlayableCharacterState : State<PlayableCharacter, PlayableCharacter.ProcessParameters>
// {
//     public PlayableCharacterState(string id,
//         Type[] behaviorTypes,
//         IEnumerable<IBehaviorProperties> behaviorProperties,
//         IIoCContainer container) : base(id, behaviorTypes, behaviorProperties, container)
//     {
//     }
// }
//
// [CreationParametersType(typeof(Parameters))]
// [StateMachine(typeof(PlayableCharacterState))]
// public class PlayableCharacter : StateGameObject<PlayableCharacter>, IScriptObject, IConsoleInfoSource
// {
//     public class Parameters
//     {
//         public double Mass { get; set; }
//         public PlayerIndex PlayerIndex { get; set; }
//     }
//
//     public ObjectAndEnvirionment<PlayableCharacter> Envirionment { get; }
//     public IPlayableCharacterInput Input { get; }
//
//     private IMaterial material;
//     private IMaterial originalMaterial;
//
//     protected PlayableCharacter(IIoCFactory iocFactory, ISimulation simulation, 
//         IEnumerable<IState> states, IEnumerable<ColliderCreationParameters> colliders,
//         Parameters parameters, CreateObjectParameters createObjectParameters)
//         : base(iocFactory, simulation, states)
//     {
//         Input = iocFactory.IoCConstruct<PlayableCharacterInput>(new PlayableCharacterInput.Parameters { PlayerIndex = Math.Max(0, (int)parameters.PlayerIndex) });
//         Envirionment = new ObjectAndEnvirionment<PlayableCharacter>(this);
//
//         material = colliders.First().Material.Clone();
//         originalMaterial = colliders.First().Material;
//
//         colliders.First().Material = material;
//
//         var bodyColliders = colliders.Select(o =>
//         {
//             o.AttachToBody = Body;
//             return simulation.CreateCollider(o);
//         }).ToArray();
//
//         Body.AddColliders(bodyColliders);
//         Body.Mass = parameters.Mass;
//         Body.Position = createObjectParameters.Position;
//
//         StateMachine.SetState(states.First().Id);
//
//         foreach (var collider in Body.Colliders)
//         {
//             collider.CollisionWith += OnCollisionWith;
//         }
//     }
//
//     private void OnCollisionWith(bool byMyMovement, ICollider other, VectorF impact)
//     {
//         ProcessParametersObj.Event = ProcessEvent.Collission;
//         ProcessParametersObj.TimeDelta = Body.Simulation.SimulationParameters.TimeDelta;
//         ProcessParametersObj.Collision = new Collision(other, impact, byMyMovement);
//
//         StateMachine.Process(ProcessParametersObj);
//     }
//
//     public void SetColliderFriction(double friction)
//     {
//         material.Friction = friction;
//     }
//
//     public override void OnFixedUpdate(out bool cancelUpdate)
//     {
//         material.Friction = originalMaterial.Friction;
//         base.OnFixedUpdate(out cancelUpdate);
//     }
//
//     public override void Dispose()
//     {
//             
//     }
//
//     public void FillInfo(StringBuilder stringBuilder)
//     {
//         stringBuilder.AppendFormat("PlayableCharacter     ");
//         stringBuilder.AppendFormat("State: {0,10} | ", StateMachine.CurrentState);
//         stringBuilder.AppendFormat("IsOnGround: {0,5} | ", Envirionment.IsOnGround ? "Yes" : "No");
//         stringBuilder.AppendFormat("Velocity: ({0:+00.00;-00.00}, {1:+00.00;-00.00}) | ", Body.Velocity.X, Body.Velocity.Y);
//         stringBuilder.Append("Flags: ");
//         Flags.GetFlags(stringBuilder);
//     }
// }