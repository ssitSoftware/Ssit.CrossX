// using EbatianoSoftware.CrossX.Primitives;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Hold Jump")]
// [Description("Accelerates jump when *Jump* button is pressed.")]
// [RequiredStates(StateNames.Raise, StateNames.Fall)]
// public class HoldJumpBehavior: PlayableCharacterBehavior
// {
//     public class Properties : IBehaviorProperties
//     {
//         public double JumpHoldAccelerationOnCurrentVelocity { get; set; } = 1.6;
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.FixedUpdate) return false;
//
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         if(character.Body.Velocity.Y >= 0)
//         {
//             character.StateMachine.SetState(StateNames.Fall);
//             return true;
//         }
//
//         if (character.Input.IsDown(GameButton.Jump))
//         {
//             character.Body.Velocity += new VectorF(0, character.Body.Velocity.Y) * properties.JumpHoldAccelerationOnCurrentVelocity * parameters.TimeDelta;
//         }
//         else
//         {
//             character.StateMachine.SetState(StateNames.Raise);
//         }
//         return false;
//     }
// }