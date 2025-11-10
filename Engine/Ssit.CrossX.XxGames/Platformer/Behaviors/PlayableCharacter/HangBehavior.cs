// using EbatianoSoftware.CrossX.Primitives;
// using System;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Platformer.Input;
// using XxGames.Platformer.Objects;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Hang")]
// [Description("Jump in the air when player presses *Jump* button.\n" +
//              "Sets air jump flag (*AlreadyAirJumpedFlag*) after jump so next air jump is imposible.")]
// [RequiredStates(StateNames.HangMove, StateNames.Hang, StateNames.Jump, StateNames.Fall)]
// public class HangBehavior : PlayableCharacterBehavior
// {
//     public class Properties : IBehaviorProperties
//     {
//         public bool CanJump { get; set; } = true;
//         public double JumpVelocity { get; set; } = 14;
//         public bool CanFallOnDemand { get; set; } = true;
//         public double Acceleration { get; set; } = 80;
//         public double MaxSpeed { get; set; } = 6;
//         public string HangColliderName { get; set; } = "Hang";
//     }
//
//     protected override void OnStateEnter(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//         var hangCollider = character.Body.FindCollider(properties.HangColliderName);
//         if (hangCollider != null)
//         {
//             hangCollider.IsActive = true;
//         }
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         switch (parameters.Event)
//         {
//             case ProcessEvent.PostFixedUpdate:
//                 character.Body.LimitVelocity(properties.MaxSpeed, 1000);
//                 return false;
//
//             case ProcessEvent.Update:
//
//                 if (character.Input.IsJustPressed(GameButton.Jump))
//                 {
//                     if (properties.CanFallOnDemand && (character.Input.AnalogValue(GameAnalog.Vertical) > 0.5 || !properties.CanJump))
//                     {
//                         character.Body.Position += new VectorF(0, 0.1);
//                         character.Body.Touch();
//                         character.StateMachine.SetState(StateNames.Fall);
//                         return true;
//                     }
//
//                     if (properties.CanJump)
//                     {
//                         character.Body.Velocity = new VectorF(character.Body.Velocity.X, -properties.JumpVelocity);
//                         character.Body.Touch();
//                         character.StateMachine.SetState(StateNames.Jump);
//                         return true;
//                     }
//                 }
//
//                 return false;
//
//             case ProcessEvent.FixedUpdate:
//
//                 var left = character.Input.IsDown(GameButton.Left);
//                 var right = character.Input.IsDown(GameButton.Right);
//
//                 var move = character.Input.AnalogValue(GameAnalog.Horizontal);
//                 move += (left ? -1 : 0) + (right ? 1 : 0);
//
//                 if (Math.Abs(move) > 0.2)
//                 {
//                     character.FacingLeft = move < 0;
//                     character.Body.ApplyForce(new VectorF(move * properties.Acceleration * character.Body.Mass, 0));
//
//                     character.StateMachine.SetState(StateNames.HangMove);
//                     return false;
//                 }
//                 character.StateMachine.SetState(StateNames.Hang);
//                 return false;
//         }
//         return false;
//     }
//
//     protected override void OnStateLeave(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//         var hangCollider = character.Body.FindCollider(properties.HangColliderName);
//         if (hangCollider != null)
//         {
//             hangCollider.IsActive = false;
//         }
//     }
// }
//
// [PropertiesType(typeof(HangBehavior.Properties))]
// [DisplayName("Start Hang")]
// [Description("Starts hanging when grabs the hanging element.\n")]
// [RequiredStates(StateNames.Hang)]
// public class StartHangBehavior : PlayableCharacterBehavior
// {
//     protected override void OnStateEnter(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<HangBehavior.Properties>();
//         var hangCollider = character.Body.FindCollider(properties.HangColliderName);
//         if (hangCollider != null)
//         {
//             hangCollider.IsActive = true;
//         }
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, StateGameObject<Objects.PlayableCharacter>.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.Collission) return false;
//
//         var properties = behaviorPropertiesContainer.Get<HangBehavior.Properties>();
//
//         var hangCollider = character.Body.FindCollider(properties.HangColliderName);
//         if (hangCollider != null && (parameters.Collision.Collider.Material.ColliderGroup & hangCollider.Material.ColliderGroup) != 0)
//         {
//             character.StateMachine.SetState(StateNames.Hang);
//             return true;
//         }
//
//         return false;
//     }
//
//     protected override void OnStateLeave(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<HangBehavior.Properties>();
//         var hangCollider = character.Body.FindCollider(properties.HangColliderName);
//         if (hangCollider != null)
//         {
//             hangCollider.IsActive = false;
//         }
//     }
// }