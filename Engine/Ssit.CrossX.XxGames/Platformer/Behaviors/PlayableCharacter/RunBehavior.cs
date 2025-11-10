// using EbatianoSoftware.CrossX.Primitives;
// using System;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Platformer.Behaviors.PlayableCharacter;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [DisplayName("Run")]
// [PropertiesType(typeof(Properties))]
// [Description("Handles left/right movement on the ground and makes player character run.\n" +
//              "Stering with *Left* or *Right* button or *Horizontal* analog.")]
// [RequiredStates(StateNames.Run, StateNames.Brake)]
// public class RunBehavior: PlayableCharacterBehavior
// {
//     public class Properties : IBehaviorProperties
//     {
//         public double Acceleration { get; set; } = 50;
//         public double IdleMaxSpeed { get; set; } = 1;
//         public double RunMaxSpeed { get; set; } = 16;
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         if (parameters.Event == ProcessEvent.PostFixedUpdate)
//         {
//             character.Body.LimitVelocity(properties.RunMaxSpeed, 1000);
//             return false;
//         }
//
//         if (parameters.Event != ProcessEvent.FixedUpdate) return false;
//
//         var left = character.Input.IsDown(GameButton.Left);
//         var right = character.Input.IsDown(GameButton.Right);
//
//         var move = character.Input.AnalogValue(GameAnalog.Horizontal);
//         move += (left ? -1 : 0) + (right ? 1 : 0);
//
//         character.Body.LimitVelocity(properties.RunMaxSpeed, 1000);
//
//         if (Math.Abs(move) > 0.01)
//         {
//             move *= properties.Acceleration;
//             character.Body.ApplyForce(new VectorF(move, 0) * character.Body.Mass);
//             character.FacingLeft = move < 0;
//             character.SetColliderFriction(0);
//             return character.StateMachine.SetState(StateNames.Run);
//         }
//
//         if (Math.Abs(character.Body.Velocity.X) < properties.IdleMaxSpeed)
//         {
//             character.StateMachine.SetState(StateNames.Idle);
//         }
//         else
//         {
//             character.StateMachine.SetState(StateNames.Brake);
//         }
//         return false;
//     }
// }