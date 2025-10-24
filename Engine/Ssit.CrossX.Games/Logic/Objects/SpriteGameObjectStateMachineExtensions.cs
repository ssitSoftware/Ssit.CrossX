using Ssit.CrossX.Games.Logic.Stering;

namespace Ssit.CrossX.Games.Logic.Objects;

public static class SpriteGameObjectStateMachineExtensions
{
    public static SpriteGameObjectStateMachine<TObject, TStateObject> WithBehaviorState<TObject, TStateObject>(
        this SpriteGameObjectStateMachine<TObject, TStateObject> sm, string name, string sequence, SteringBehavior<TStateObject>[] behaviors) where TObject: SpriteGameObject2, TStateObject
    {
        var state = new SteringStateWithBehaviors<TStateObject>(name, behaviors);
        
        return sm
            .RegisterState(state)
            .MapSequence(name, sequence);
    }

    public static SpriteGameObjectStateMachine<TObject, TStateObject> WithBehaviorState<TObject, TStateObject>(
        this SpriteGameObjectStateMachine<TObject, TStateObject> sm, string name, SteringBehavior<TStateObject>[] behaviors)
        where TObject : SpriteGameObject2, TStateObject =>
        sm.WithBehaviorState(name, name, behaviors);
}