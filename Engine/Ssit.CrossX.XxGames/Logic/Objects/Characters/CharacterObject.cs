using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Audio;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Platformer.Builders;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public class CharacterObject<TCharacter> : SpriteGameObject2, ISteringCharacter where TCharacter: CharacterObject<TCharacter>
{
    public CharacterSteringParameters SteringParameters { get; } = new();
    public ISteringInput SteringInput { get; protected init; }
    public Vector2 MomentumOffset { get; set; }
    
    public ICharacterPhysicsValues PhysicsValues { get; protected init; }
    protected ContextSoundContainer SoundContainer { get; init; }
    
    public SpriteGameObjectStateMachine<TCharacter, ISteringCharacter> StateMachine { get; protected set; }
    
    TParameters IGameObject.Get<TParameters>() => GetParameter<TParameters>();
    SteringState<ISteringCharacter> ISteringCharacter.CurrentSteringState => StateMachine.SteringStateMachine.CurrentState;
    
    protected virtual TParameters GetParameter<TParameters>() => throw new ArgumentOutOfRangeException();
    
    public void SetSteringState(string name) => StateMachine.SetSteringState(name);
    
    ContextSoundContainer IGameObject.SoundContainer => SoundContainer;
    
    protected CharacterObject(GameObjectsServices services, ObjectCreationParameters parameters): base(services, parameters)
    {
    }
    
    protected void InitializeStateMachine()
    {
        StateMachine = new SpriteGameObjectStateMachine<TCharacter, ISteringCharacter>((TCharacter)this);
        StateMachine.SteringStateMachine.OnStateChanged += SteringStateMachineOnOnStateChanged;
    }
    
    protected void DetectOnGround()
    {
        this.DetectOnGround(out SteringParameters.IsOnGround, out SteringParameters.IsOnPlatform, out SteringParameters.IsOnStaticGround, out SteringParameters.GroundMaterial);
    }
    
    private static void SteringStateMachineOnOnStateChanged(object sender, SteringState<ISteringCharacter> _)
    {
        var sm = (SteringStateMachine<ISteringCharacter>)sender;
        ((CharacterObject<TCharacter>)sm.Object).DetectOnGround();
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        
        StateMachine.SteringStateMachine.OnStateChanged -= SteringStateMachineOnOnStateChanged;
        SoundContainer?.Dispose();
    }
}