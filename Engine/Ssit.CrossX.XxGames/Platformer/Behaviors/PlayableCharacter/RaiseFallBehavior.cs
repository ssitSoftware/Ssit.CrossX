// using System;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [DisplayName("Raise & Fall Analizer")]
// [RequiredStates("Fall", "Raise")]
// [Description("Updates player character if its going up (Raising) or down (Falling).\n" +
//              "Resets character to *Idle* or *Brake* state when on ground.")]
// public class RaiseFallBehavior : PlayableCharacterBehavior
// {
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.FixedUpdate) return false;
//
//         var dir = -character.Body.Simulation.SimulationParameters.GravityAcceleration.Normalized();
//         var upVelocity = dir.Dot(character.Body.Velocity);
//
//         if (character.Envirionment.IsOnGround)
//         {
//             if (character.StateMachine.CurrentState == StateNames.Fall || character.StateMachine.CurrentState == StateNames.Raise)
//             {
//                 if (Math.Abs(character.Body.Velocity.X) < 0.1)
//                 {
//                     character.StateMachine.SetState(StateNames.Idle);
//                 }
//                 else
//                 {
//                     character.StateMachine.SetState(StateNames.Brake);
//                 }
//                 return true;
//             }
//             return false;
//         }
//
//         if (upVelocity < 0)
//         {
//             character.StateMachine.SetState(StateNames.Fall);
//         }
//         else if(upVelocity > 0)
//         {
//             character.StateMachine.SetState(StateNames.Raise);
//         }
//
//         return false;
//     }
// }