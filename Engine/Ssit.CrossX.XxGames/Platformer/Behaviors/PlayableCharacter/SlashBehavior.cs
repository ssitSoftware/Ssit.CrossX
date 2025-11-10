// using EbatianoSoftware.CrossX.Async;
// using XxGames.Logic.Attributes;
//
// namespace XxGames.Platformer.Behaviors.PlayableCharacter;
//
// [PropertiesType(typeof(Properties))]
// [DisplayName("Slash with melee weapon")]
// [Description("Handles slash with the sword etc.\n")]
// [RequiredStates(StateNames.Slash)]
// public class SlashBehavior : SlashBehaviorBase<SlashBehavior.Properties>
// { 
//     public class Properties: PropertiesBase{}
//     public SlashBehavior() : base(false) { }
// }
//
// [PropertiesType(typeof(SlashBehavior.Properties))]
// [Description("Handles slash state.\n")]
// [RequiredStates(StateNames.Fall, StateNames.Idle, StateNames.Brake, StateNames.Raise)]
// [OptionalStates(StateNames.SlashCombo1, StateNames.SlashCombo2, StateNames.SlashCombo3, StateNames.SlashCombo4)]
// public class SlashingStateBehavior : SlashingStateBehaviorBase<AirSlashBehavior.Properties>
// {
//     public SlashingStateBehavior() : base(false) { }
// }