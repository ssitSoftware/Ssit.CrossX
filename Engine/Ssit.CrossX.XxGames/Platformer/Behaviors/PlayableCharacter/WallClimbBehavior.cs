// using EbatianoSoftware.CrossX.Primitives;
// using System;
// using System.Collections.Generic;
// using Ssit.CrossX.XxGames.Platformer;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.PhysicsCore;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Wall Climb")]
// [Description("Handles climbing on the wall.")]
// [RequiredStates(StateNames.Idle, StateNames.Fall, StateNames.Raise, StateNames.WallClimb, StateNames.WallClimbingDown, StateNames.WallClimbingUp)]
// public class WallClimbBehavior: PlayableCharacterBehavior
// {
//     public const string LeftFlag = nameof(WallClimbBehavior) + ":" +  nameof(LeftFlag);
//
//     private readonly List<ICollider> colliders = new List<ICollider>();
//
//     public class Properties : IBehaviorProperties
//     {
//         public IMaterial[] ClimbingMaterials { get; set; }
//         public IMaterial[] NoClimbingMaterials { get; set; }
//         public double JumpVerticalVelocity { get; set; } = 16;
//         public double JumpHorizontalVelocity { get; set; } = 10;
//         public double ReleaseHorizontalVelocity { get; set; } = 2;
//         public double ClimbingSpeed { get; set; } = 6;
//         public bool ReleaseWhenBelow { get; set; } = true;
//     }
//
//     protected override void OnStateEnter(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         character.Body.IsKinematic = true;
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         switch (parameters.Event )
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
//             {
//                 if(character.Envirionment.IsOnGround)
//                 {
//                     character.StateMachine.SetState(StateNames.Idle);
//                     return true;
//                 }
//
//                 var speed = character.Input.AnalogValue(GameAnalog.Vertical);
//                 var left = character.Flags.HasFlag(LeftFlag);
//
//                 if (Math.Abs(speed) > 0.1)
//                 {
//                     colliders.Clear();
//
//                     var bounds = character.Body.Colliders[0].Aabb;
//
//                     if (left)
//                     {
//                         bounds.Left -= 0.01;
//                     }
//                     else
//                     {
//                         bounds.Right += 0.01;
//                     }
//                             
//                     character.Body.Simulation.GetColliders(bounds, colliders);
//
//                     for(var idx = 0; idx < colliders.Count; )
//                     {
//                         if( !MaterialCheckHelper.MatchMaterial(colliders[idx].Material, properties.ClimbingMaterials, properties.NoClimbingMaterials))
//                         {
//                             colliders.RemoveAt(idx);
//                             continue;
//                         }
//                         ++idx;
//                     }
//
//                     if(colliders.Count == 0 )
//                     {
//                         character.StateMachine.SetState(StateNames.Fall);
//                         return true;
//                     }
//
//                     var maxY = double.MinValue;
//                     var minY = double.MaxValue;
//
//                     var height = character.Body.Colliders[0].GetAabb(PointF.Zero).Height;
//
//                     for (var idx =0; idx < colliders.Count; ++idx)
//                     {
//                         var aabb = colliders[idx].Aabb;
//
//                         maxY = Math.Max(aabb.Bottom, maxY);
//                         minY = Math.Min(aabb.Top, minY);
//                     }
//
//                     minY += height / 2;
//                     maxY -= height / 2;
//
//                     var move = speed * properties.ClimbingSpeed * parameters.TimeDelta;
//                     var afterMove = character.Body.Position.Y + move;
//
//                     if (properties.ReleaseWhenBelow)
//                     {
//                         if(afterMove > maxY)
//                         {
//                             character.Body.Velocity = new VectorF(left ? properties.ReleaseHorizontalVelocity : -properties.ReleaseHorizontalVelocity, 0);
//                             character.Body.Touch();
//                             character.StateMachine.SetState(StateNames.Fall);
//                             return true;
//                         }
//                     }
//
//                     var avaliableMove = Math.Max(Math.Min(maxY, afterMove), minY);
//
//                     move = avaliableMove - character.Body.Position.Y;
//                     if (Math.Abs(move) > 0.01)
//                     {
//                         character.Body.KinematicMove(new VectorF(0, move), true);
//                         character.StateMachine.SetState(move > 0 ? StateNames.WallClimbingDown : StateNames.WallClimbingUp);
//                         if (left) character.Flags.Set(LeftFlag);
//                         return true;
//                     }
//                 }
//
//                 character.StateMachine.SetState(StateNames.WallClimb);
//                 if (left) character.Flags.Set(LeftFlag);
//             }
//                 return true;
//         }
//             
//
//         return false;
//     }
//
//     protected override void OnStateLeave(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         character.Body.IsKinematic = false;
//         character.Flags.Reset(LeftFlag);
//     }
// }
//
// [PropertiesType(typeof(WallClimbBehavior.Properties))]
// [DisplayName("Start Wall Climb")]
// [Description("Enables wall climbing for player character.")]
// [RequiredStates(StateNames.WallClimb)]
// public class StartWallClimbBehavior : PlayableCharacterBehavior
// {
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.Collission) return false;
//
//         var properties = behaviorPropertiesContainer.Get<WallClimbBehavior.Properties>();
//         var collision = parameters.Collision;
//
//
//         var shouldClimb = MaterialCheckHelper.MatchMaterial(collision.Collider.Material, properties.ClimbingMaterials, properties.NoClimbingMaterials);
//         if (!shouldClimb) return false;
//
//         var aabb = character.Body.Colliders[0].Aabb;
//         var obstacleAabb = collision.Collider.Aabb;
//
//         if (aabb.Bottom > obstacleAabb.Bottom) return false;
//         if (aabb.Top < obstacleAabb.Top) return false;
//
//         bool isLeft = collision.Impact.X < 0 ^ collision.ByMovement;
//
//         if (character.FacingLeft != isLeft) return false;
//
//         if (isLeft)
//         {
//             character.Flags.Set(WallClimbBehavior.LeftFlag);
//         }
//         character.Body.Velocity = VectorF.Zero;
//         character.StateMachine.SetState(StateNames.WallClimb);
//         return true;
//     }
// }