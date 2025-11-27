using Ssit.CrossX.XxGames.Logic.Objects.Characters;

namespace Ssit.CrossX.XxGames.Logic.AI;

public static class AiStateMachineExtensions
{
    public static AiStateMachine<TStateObject> WithBehaviorState<TStateObject>(
        this AiStateMachine<TStateObject> sm, string name, AiBehavior<TStateObject>[] behaviors) where TStateObject: IAiControlledCharacter
    {
        var state = new AiStateWithBehaviors<TStateObject>(name, behaviors);

        sm.RegisterState(state);
        return sm;
    }
}