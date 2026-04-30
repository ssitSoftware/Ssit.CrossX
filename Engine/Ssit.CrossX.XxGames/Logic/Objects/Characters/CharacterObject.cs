using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.XxGames.Audio;
using Ssit.CrossX.XxGames.Logic.Stering;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Platformer.Builders;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public abstract class CharacterObject<TCharacter> : SpriteGameObject2, IBodyEventsReceiver, IActivationHandler, ISteringCharacter where TCharacter: CharacterObject<TCharacter>
{
    TParameters IGameObject.Get<TParameters>(bool create) => GetParameters<TParameters>(create);
    SteringState<ISteringCharacter> ISteringCharacter.CurrentSteringState => SteringStateMachine.InternalStateMachine.CurrentState;
    
    ISteringInput ISteringCharacter.SteringInput => SteringInput;
    
    public CharacterSteringParameters SteringParameters { get; } = new();
    
    protected abstract ISteringInput SteringInput { get; }
    
    public Vector2 MomentumOffset { get; set; }
    protected Vector2 GroundDetectionEpsilon { get; set; } = Vector2.Zero;
    
    public ICharacterPhysicsValues PhysicsValues { get; protected init; }
    protected ContextSoundContainer SoundContainer { get; init; }

    protected SpriteGameObjectStateMachine<TCharacter, ISteringCharacter> SteringStateMachine { get; private set; }

    private readonly Dictionary<Type, object> _parameters = new();

    TParameters ICharacter.GetParameters<TParameters>(bool create) => GetParameters<TParameters>(create);
    
    protected virtual TParameters GetParameters<TParameters>(bool create)
    {
        if (create)
        {
            if (!_parameters.TryGetValue(typeof(TParameters), out var obj))
            {
                obj = Activator.CreateInstance<TParameters>();
                _parameters.Add(typeof(TParameters), obj);
            }
            return (TParameters)obj;
        }

        throw new Exception("No parameters of given type.");
    }
    
    void IBodyEventsReceiver.OnCollision(ICollider source, ICollider other, Vector2 impact)
    {
        SteringStateMachine.InternalStateMachine.CurrentState?.Collission((TCharacter)this, source, other, impact);
    }
    
    public void SetSteringState(string name) => SteringStateMachine.SetSteringState(name);
    
    ContextSoundContainer IGameObject.SoundContainer => SoundContainer;
    
    protected CharacterObject(GameObjectsServices services, ObjectCreationParameters parameters): base(services, parameters)
    {
        Body.AddEventsReceiver(this);
    }
    
    protected void InitializeSteringStateMachine()
    {
        SteringStateMachine = new SpriteGameObjectStateMachine<TCharacter, ISteringCharacter>((TCharacter)this);
        SteringStateMachine.InternalStateMachine.OnStateChanged += SteringStateMachineOnOnStateChanged;
    }
    
    protected void DetectOnGround()
    {
        this.DetectOnGround(out SteringParameters.IsOnGround, out SteringParameters.IsOnPlatform, out SteringParameters.IsOnStaticGround, out SteringParameters.GroundMaterial, GroundDetectionEpsilon);
    }
    
    private static void SteringStateMachineOnOnStateChanged(object sender, SteringState<ISteringCharacter> _)
    {
        var sm = (SteringStateMachine<ISteringCharacter>)sender;
        ((CharacterObject<TCharacter>)sm.Object).DetectOnGround();
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        
        SteringStateMachine.InternalStateMachine.OnStateChanged -= SteringStateMachineOnOnStateChanged;
        SoundContainer?.Dispose();
    }

    void IActivationHandler.Activate(bool active)
    {
        if (!active && SoundContainer is not null)
        {
            SoundContainer.StopAll();
        }
    }
}