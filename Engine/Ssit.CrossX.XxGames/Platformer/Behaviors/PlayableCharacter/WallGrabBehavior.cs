// using EbatianoSoftware.CrossX.Primitives;
// using System;
// using System.Collections.Generic;
// using Ssit.CrossX.XxGames.Platformer;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.PhysicsCore;
// using XxGames.Platformer.Behaviors.PlayableCharacter;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Wall Grab")]
// [Description("Handles grabbing the wall to slow down fall.")]
// [RequiredStates(StateNames.Raise, StateNames.Fall)]
// public class WallGrabBehavior : PlayableCharacterBehavior
// {
//     public const string LeftFlag = nameof(WallGrabBehavior) + ":" + nameof(LeftFlag);
//     public const string PushTimer = nameof(WallGrabBehavior) + ":" + nameof(PushTimer);
//
//     private readonly List<ICollider> colliders = new List<ICollider>();
//
//     public class Properties : IBehaviorProperties
//     {
//         public IMaterial[] GrabbingMaterials { get; set; }
//         public IMaterial[] NoGrabbingMaterials { get; set; }
//         public double WallPushForce { get; set; } = 400;
//         public double JumpVerticalVelocity { get; set; } = 16;
//         public double JumpHorizontalVelocity { get; set; } = 15;
//         public double ReleaseHorizontalVelocity { get; set; } = 4;
//         public double ReleaseTime { get; set; } = 0.2;
//         public double BlockSteringTimeAfterRelease { get; set; } = 0.2;
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         switch (parameters.Event)
//         {
//             case ProcessEvent.Update:
//                 if (character.Input.IsJustPressed(GameButton.Jump))
//                 {
//                     var left = character.Flags.HasFlag(LeftFlag);
//                     bool fall = character.Input.AnalogValue(GameAnalog.Vertical) > 0.5;
//
//                     var horizontalVelocity = fall ? properties.ReleaseHorizontalVelocity : properties.JumpHorizontalVelocity;
//
//                     character.Body.Velocity = new VectorF(left ? horizontalVelocity : -horizontalVelocity, fall ? 0 : -properties.JumpVerticalVelocity);
//                     character.Body.Touch();
//                     character.StateMachine.SetState(character.Body.Velocity.Y < 0 ? StateNames.Raise : StateNames.Fall);
//                     return true;
//                 }
//                 break;
//
//             case ProcessEvent.FixedUpdate:
//
//                 if (character.Envirionment.IsOnGround)
//                 {
//                     character.StateMachine.SetState(StateNames.Idle);
//                     return true;
//                 }
//
//             {
//                 var left = character.Flags.HasFlag(LeftFlag);
//                 colliders.Clear();
//                 var bounds = character.Body.Colliders[0].Aabb;
//
//                 if (left)
//                 {
//                     bounds.Left -= 0.01;
//                 }
//                 else
//                 {
//                     bounds.Right += 0.01;
//                 }
//
//                 character.Body.Simulation.GetColliders(bounds, colliders);
//
//                 for (var idx = 0; idx < colliders.Count;)
//                 {
//                     if (!MaterialCheckHelper.MatchMaterial(colliders[idx].Material, properties.GrabbingMaterials, properties.NoGrabbingMaterials))
//                     {
//                         colliders.RemoveAt(idx);
//                         continue;
//                     }
//                     ++idx;
//                 }
//
//                 bool fallOff = false;
//
//                 var grabLeft = character.Input.IsDown(GameButton.GrabLeft);
//                 var grabRight = character.Input.IsDown(GameButton.GrabRight);
//                 var timer = character.Values.Get(PushTimer, -1);
//
//                 bool push = grabRight && !left || grabLeft && left;
//
//                 if (!push || timer > 0)
//                 {
//                     if (timer < 0)
//                     {
//                         timer = properties.ReleaseTime;
//                     }
//
//                     timer -= parameters.TimeDelta;
//                     character.Values.Set(PushTimer, timer);
//
//                     if (timer <= 0)
//                     {
//                         fallOff = true;
//                     }
//                 }
//
//                 if (colliders.Count == 0 || fallOff)
//                 {
//                     character.StateMachine.SetState(StateNames.Fall);
//                     character.Body.Velocity = new VectorF(left ? properties.ReleaseHorizontalVelocity : -properties.ReleaseHorizontalVelocity, 0);
//                     return true;
//                 }
//
//
//                 var maxY = double.MinValue;
//                 var height = character.Body.Colliders[0].GetAabb(PointF.Zero).Height;
//
//                 for (var idx = 0; idx < colliders.Count; ++idx)
//                 {
//                     var aabb = colliders[idx].Aabb;
//                     maxY = Math.Max(aabb.Bottom, maxY);
//                 }
//
//                 maxY -= height / 2;
//
//                 if (character.Body.Position.Y > maxY)
//                 {
//                     character.Body.Velocity = new VectorF(left ? properties.ReleaseHorizontalVelocity : -properties.ReleaseHorizontalVelocity, 0);
//                     character.Body.Touch();
//                     character.StateMachine.SetState(StateNames.Fall);
//                     return true;
//                 }
//
//                 character.Body.ApplyForce(new VectorF(left ? -properties.WallPushForce : properties.WallPushForce, 0));
//             }
//                 break;
//         }
//         return false;
//     }
//
//     protected override void OnStateLeave(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//         character.Values.Remove(PushTimer);
//         character.Values.Set(InAirSteringBehavior.BlockSteringTimer, properties.BlockSteringTimeAfterRelease);
//     }
// }
//
// [PropertiesType(typeof(WallGrabBehavior.Properties))]
// [DisplayName("Start Wall Grab")]
// [Description("Enables wall grab for player character.")]
// [RequiredStates(StateNames.WallGrab)]
// public class StartWallGrabBehavior : PlayableCharacterBehavior
// {
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.Collission) return false;
//
//         var properties = behaviorPropertiesContainer.Get<WallGrabBehavior.Properties>();
//         var collision = parameters.Collision;
//
//         var shouldGrab = MaterialCheckHelper.MatchMaterial(collision.Collider.Material, properties.GrabbingMaterials, properties.NoGrabbingMaterials);
//         if (!shouldGrab) return false;
//
//         var aabb = character.Body.Colliders[0].Aabb;
//         var obstacleAabb = collision.Collider.Aabb;
//
//         if (aabb.Bottom > obstacleAabb.Bottom) return false;
//         if (aabb.Top < obstacleAabb.Top) return false;
//
//         bool isLeft = collision.Impact.X < 0 ^ collision.ByMovement;
//
//         var grabLeft = character.Input.IsDown(GameButton.GrabLeft);
//         var grabRight = character.Input.IsDown(GameButton.GrabRight);
//
//         if (!grabLeft && isLeft) return false;
//         if (!grabRight && !isLeft) return false;
//
//         if (isLeft)
//         {
//             character.Flags.Set(WallGrabBehavior.LeftFlag);
//         }
//         character.StateMachine.SetState(StateNames.WallGrab);
//         return true;
//     }
// }