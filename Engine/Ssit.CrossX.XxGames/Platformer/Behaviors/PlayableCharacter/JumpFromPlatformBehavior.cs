// using EbatianoSoftware.CrossX.Primitives;
// using System.Linq;
// using XxGames.Logic;
// using XxGames.Logic.Attributes;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [DisplayName("Jump from platform")]
// [Description("Jumps off from platform when *Jump* button is pressed and down is hold.")]
// [RequiredStates(StateNames.Fall)]
// public class JumpFromPlatformBehavior: PlayableCharacterBehavior
// {
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.Update) return false;
//
//         if (character.Input.IsJustPressed(GameButton.Jump) && character.Input.AnalogValue(GameAnalog.Vertical) > 0.5)
//         {
//             if (character.Envirionment.IsOnGround)
//             {
//                 var colliders = character.Envirionment.LastGroundCollisions;
//                 if (colliders.All( o=>o.Material.Sides == PhysicsCore.ColliderSides.Top))
//                 {
//                     character.Body.Position += new VectorF(0, 0.1);
//                     character.Body.Touch();
//                     character.Envirionment.Recalculate();
//                     character.StateMachine.SetState(StateNames.Fall);
//                 }
//                 return true;
//             }
//         }
//         return false;
//     }
// }