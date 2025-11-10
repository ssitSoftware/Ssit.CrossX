// using EbatianoSoftware.CrossX.Async;
// using EbatianoSoftware.CrossX.Primitives;
// using System.Collections.Generic;
// using XxGames.Logic;
// using XxGames.Platformer.Input;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// public abstract class SlashBehaviorBase<TSlashProperties> : PlayableCharacterBehavior where TSlashProperties : SlashBehaviorBase<TSlashProperties>.PropertiesBase
// {
//     public class PropertiesBase : IBehaviorProperties
//     {
//         public double AddSpeed { get; set; } = 32;
//         public double MaxSpeed { get; set; } = 32;
//         public int MaxCombo { get; set; } = 0;
//         public double BodyFrictionOnSlide { get; set; } = 4;
//         public bool ForceHorizontalSlash { get; set; } = true;
//     }
//
//     private bool inAir = false;
//     protected SlashBehaviorBase(bool inAir) { this.inAir = inAir; }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.FixedUpdate) return false;
//
//         var properties = behaviorPropertiesContainer.Get<TSlashProperties>();
//
//         if(character.Input.IsJustPressed(GameButton.Slash))
//         {
//             character.Body.Velocity += new VectorF( character.FacingLeft ? -properties.AddSpeed : properties.AddSpeed, 0);
//             character.Body.LimitVelocity(properties.MaxSpeed, properties.ForceHorizontalSlash ? 0 : 1000);
//             character.StateMachine.SetState(inAir ? StateNames.AirSlash : StateNames.Slash);
//             return true;
//         }
//
//         return false;
//     }
// }
//
// public abstract class SlashingStateBehaviorBase<TSlashProperties> : PlayableCharacterBehavior where TSlashProperties: SlashBehaviorBase<TSlashProperties>.PropertiesBase
// {
//     private bool inAir = false;
//
//     protected SlashingStateBehaviorBase(bool inAir) 
//     {
//         this.inAir = inAir; 
//     }
//
//     protected override bool OnEvent(Objects.PlayableCharacter character, string eventName, IParameter[] parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         switch(eventName)
//         {
//             case "Continue":
//                 return true;
//
//             case "End":
//                 character.StateMachine.SetState(character.Envirionment.IsOnGround ? StateNames.Brake : (character.Body.Velocity.Y > 0 ? StateNames.Fall : StateNames.Raise));
//                 return true;
//         }
//
//         return base.OnEvent(character, eventName, parameters, behaviorPropertiesContainer);
//     }
//
//     protected override bool Process(Objects.PlayableCharacter character, Objects.PlayableCharacter.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         if (parameters.Event != ProcessEvent.FixedUpdate) return false;
//
//         var properties = behaviorPropertiesContainer.Get<TSlashProperties>();
//
//         character.Body.LimitVelocity(properties.MaxSpeed, properties.ForceHorizontalSlash ? 0 : 1000);
//         character.SetColliderFriction(properties.BodyFrictionOnSlide);
//         return false;
//     }
// }