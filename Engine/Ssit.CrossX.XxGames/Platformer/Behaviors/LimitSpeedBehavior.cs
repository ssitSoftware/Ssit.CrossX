// namespace Ssit.CrossX.XxGames.Platformer.Behaviors;
//
// public abstract class LimitSpeedBehavior<TObject, TAdditionalParameters>: Behavior<TObject, StateGameObject<TObject>.ProcessParameters> where TObject: StateGameObject<TObject>
// {
//     public class Properties: IBehaviorProperties
//     {
//         public double MaxHorizontalSpeed;
//         public double MaxVerticalSpeed;
//     }
//
//     protected override bool Process(TObject @object, StateGameObject<TObject>.ProcessParameters parameters, IBehaviorPropertiesContainer behaviorPropertiesContainer)
//     {
//         var properties = behaviorPropertiesContainer.Get<Properties>();
//         @object.Body.LimitVelocity(properties.MaxHorizontalSpeed, properties.MaxVerticalSpeed);
//         return false;
//     }
// }