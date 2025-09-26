using Ssit.CrossX.Games.Logic.Stering;

namespace Ssit.CrossX.Games.Logic.Objects;

public static class SpriteGameObjectStateMachineExtensions
{
    public static SpriteGameObjectStateMachine<TObject> CreateStateMachine<TObject>(this TObject obj) where TObject: SpriteGameObject2
    {
        return new SpriteGameObjectStateMachine<TObject>(obj);
    }

    public static SpriteGameObjectStateMachine<TObject> WithBehaviorState<TObject>(
        this SpriteGameObjectStateMachine<TObject> sm, string name, string sequence, SteringBehavior<TObject>[] behaviors) where TObject: SpriteGameObject2
    {
        var state = new SteringStateWithBehaviors<TObject>(name, behaviors);
        
        return sm
            .RegisterState(state)
            .MapSequence(name, sequence);
    }

    public static SpriteGameObjectStateMachine<TObject> WithBehaviorState<TObject>(
        this SpriteGameObjectStateMachine<TObject> sm, string name, SteringBehavior<TObject>[] behaviors)
        where TObject : SpriteGameObject2 =>
        sm.WithBehaviorState(name, name, behaviors);
}