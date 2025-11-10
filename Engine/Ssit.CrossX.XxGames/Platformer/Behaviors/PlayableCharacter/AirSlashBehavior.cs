// using EbatianoSoftware.CrossX.Async;
// using XxGames.Logic.Attributes;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Slash with melee weapon in air")]
// [Description("Handles slash with the sword etc. while in the air\n")]
// [RequiredStates(StateNames.AirSlash)]
// public class AirSlashBehavior : SlashBehaviorBase<AirSlashBehavior.Properties>
// {
//     public class Properties : PropertiesBase
//     {
//     }
//
//     public AirSlashBehavior() : base(true) { }
// }
//
// [PropertiesType(typeof(AirSlashBehavior.Properties))]
// [Description("Handles slash state.\n")]
// [RequiredStates(StateNames.Fall, StateNames.Idle, StateNames.Brake, StateNames.Raise)]
// [OptionalStates(StateNames.AirSlashCombo1, StateNames.AirSlashCombo2, StateNames.AirSlashCombo3, StateNames.AirSlashCombo4)]
// public class AirSlashingStateBehavior : SlashingStateBehaviorBase<AirSlashBehavior.Properties>
// {
//     public AirSlashingStateBehavior() : base(true) { }
// }