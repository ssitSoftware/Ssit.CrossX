// using System.ComponentModel;
// using EbatianoSoftware.CrossX.Primitives;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Air Jump")]
// [Description("Jump in the air when player presses *Jump* button.\n" +
//              "Sets air jump flag (*AlreadyAirJumpedFlag*) after jump so next air jump is imposible.")]
// [RequiredStates(StateNames.Raise)]
// public class AirJumpBehavior : PlayableCharacterBehavior
// {
//     public const string AlreadyJumpedFlag = nameof(AirJumpBehavior) + ":" + nameof(AlreadyJumpedFlag);
//
//     public class Properties : IBehaviorProperties
//     {
//         public double AirJumpVelocity { get; set; } = 15;
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
//             if (!character.Flags.HasFlag(AlreadyJumpedFlag))
//             {
//                 character.Flags.Set(AlreadyJumpedFlag);
//                 character.Body.Velocity = new VectorF(character.Body.Velocity.X, -properties.AirJumpVelocity);
//                 character.Body.Touch();
//                 character.StateMachine.SetState(StateNames.Raise);
//                 return true;
//             }
//         }
//
//         return false;
//     }
// }
//
// [Description("Resets air jump flag (*AlreadyAirJumpedFlag*) to make air jump possible again.\n" +
//              "It should be added to on ground states if air jump is supported.")]
// public class ResetAirJumpFlagBehavior : PlayableCharacterBehavior
// {
//     protected override void OnStateEnter(Objects.PlayableCharacter character, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         character.Flags.Reset(AirJumpBehavior.AlreadyJumpedFlag);
//     }
// }