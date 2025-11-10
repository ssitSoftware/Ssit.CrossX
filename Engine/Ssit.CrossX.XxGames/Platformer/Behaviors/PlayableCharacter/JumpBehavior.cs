// using EbatianoSoftware.CrossX.Primitives;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Jump")]
// [Description("Jumps when player character is on ground after *Jump* button pressed.")]
// [RequiredStates(StateNames.Jump)]
// public class JumpBehavior : PlayableCharacterBehavior
// {
//     public class Properties : IBehaviorProperties
//     {
//         public double JumpVelocity { get; set; } = 15;
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.Update) return false;
//
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         if(character.Input.IsJustPressed(GameButton.Jump))
//         {
//             if(character.Envirionment.IsOnGround)
//             {
//                 character.Body.Velocity = new VectorF(character.Body.Velocity.X, -properties.JumpVelocity);
//                 character.Body.Touch();
//                 character.StateMachine.SetState(StateNames.Jump);
//                 return true;
//             }
//         }
//         return false;
//     }
// }