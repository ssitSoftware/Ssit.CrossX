// using EbatianoSoftware.CrossX.Primitives;
// using System;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("In Air Stering")]
// [Description("Enables stering while in the air.")]
// public class InAirSteringBehavior : PlayableCharacterBehavior
// {
//     public const string BlockSteringTimer = nameof(InAirSteringBehavior) + ":" + nameof(BlockSteringTimer);
//     public class Properties : IBehaviorProperties
//     {
//         public double Acceleration { get; set; } = 30;
//         public bool SwitchFacing { get; set; } = true;
//         public double MaxSpeed { get; set; } = 16;
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.FixedUpdate) return false;
//
//         var timer = character.Values.Get(BlockSteringTimer, 0);
//         if(timer > 0)
//         {
//             timer -= parameters.TimeDelta;
//             character.Values.Set(BlockSteringTimer, timer);
//             if (timer <= 0)
//             {
//                 character.Values.Remove(BlockSteringTimer);
//             }
//             return false;
//         }
//
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//
//         var left = character.Input.IsDown(GameButton.Left);
//         var right = character.Input.IsDown(GameButton.Right);
//
//         var move = character.Input.AnalogValue(GameAnalog.Horizontal);
//         move += (left ? -1 : 0) + (right ? 1 : 0);
//
//         character.Body.LimitVelocity(properties.MaxSpeed, 1000);
//         if (Math.Abs(move) > 0.01)
//         {
//             move *= properties.Acceleration;
//             character.Body.ApplyForce(new VectorF(move, 0) * character.Body.Mass);
//             if (properties.SwitchFacing)
//             {
//                 character.FacingLeft = move < 0;
//             }
//             return true;
//         }
//         return false;
//     }
// }