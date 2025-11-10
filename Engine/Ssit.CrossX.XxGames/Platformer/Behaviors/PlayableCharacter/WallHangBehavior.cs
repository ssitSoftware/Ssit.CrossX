// using EbatianoSoftware.CrossX.Primitives;
// using Ssit.CrossX.XxGames.Platformer;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.PhysicsCore;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Wall Hang")]
// [Description("Handles hanging on the wall.")]
// [RequiredStates(StateNames.Raise)]
// public class WallHangBehavior: PlayableCharacterBehavior
// {
//     public const string Left = nameof(WallHangBehavior) + ":" + nameof(Left);
//
//     public class Properties : IBehaviorProperties
//     {
//         public IMaterial[] HangingMaterials { get; set; }
//         public IMaterial[] NoHangingMaterials { get; set; }
//         public double JumpVerticalVelocity { get; set; }
//         public double JumpHorizontalVelocity { get; set; }
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.Update) return false;
//
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         if (character.Input.IsJustPressed(GameButton.Jump))
//         {
//             var left = character.Flags.HasFlag(Left);
//
//             character.Body.Velocity = new VectorF(left ? properties.JumpHorizontalVelocity : -properties.JumpHorizontalVelocity, -properties.JumpVerticalVelocity);
//             character.Body.Touch();
//             character.StateMachine.SetState(StateNames.Raise);
//             return true;
//         }
//         return false;
//     }
//
//     protected override void OnStateLeave(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         character.Body.IsKinematic = false;
//         character.Flags.Reset(Left);
//     }
// }
//
// [PropertiesType(typeof(WallHangBehavior.Properties))]
// [DisplayName("Start Wall Hang")]
// [Description("Enables wall hanging for player character.")]
// [RequiredStates(StateNames.WallHang)]
// public class StartWallHangBehavior : PlayableCharacterBehavior
// {
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.Collission) return false;
//
//         var properties = behaviorPropertiesContainer.Get<WallHangBehavior.Properties>();
//         var collision = parameters.Collision;
//
//         var shouldHang = MaterialCheckHelper.MatchMaterial(collision.Collider.Material, properties.HangingMaterials, properties.NoHangingMaterials);
//         if (!shouldHang) return false;
//
//         var aabb = character.Body.Colliders[0].Aabb;
//         var obstacleAabb = collision.Collider.Aabb;
//
//         if (aabb.Bottom > obstacleAabb.Bottom) return false;
//         if (aabb.Top < obstacleAabb.Top) return false;
//
//         bool isLeft = collision.Impact.X < 0 ^ collision.ByMovement;
//
//         if (isLeft)
//         {
//             character.Flags.Set(WallHangBehavior.Left);
//         }
//         character.Body.IsKinematic = true;
//         character.StateMachine.SetState(StateNames.WallHang);
//         return true;
//     }
// }