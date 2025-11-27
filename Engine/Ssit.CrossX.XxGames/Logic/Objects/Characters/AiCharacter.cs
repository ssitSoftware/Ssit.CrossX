using System.Numerics;
using Ssit.CrossX.XxGames.Logic.AI;
using Ssit.CrossX.XxGames.Platformer.Builders;

namespace Ssit.CrossX.XxGames.Logic.Objects.Characters;

public abstract class AiCharacter<TCharacter> : CharacterObject<TCharacter>, IAiControlledCharacter, IHittable
    where TCharacter : AiCharacter<TCharacter>
{
    protected AiStateMachine<IAiControlledCharacter> AiStateMachine { get; }
    private readonly AiSteringInput _aiSteringInput = new();

    AiSteringInput IAiControlledCharacter.AiSteringInput => _aiSteringInput;
    ISteringCharacter IAiControlledCharacter.SteringCharacter => this;

    Vector2 IHittable.Position => Body.Position;
    bool IHittable.Hit( Vector2 dir, float power ) => OnHit(dir, power);
    
    protected override ISteringInput SteringInput => _aiSteringInput;

    protected AiCharacter(GameObjectsServices services, ObjectCreationParameters parameters) : base(services, parameters)
    {
        AiStateMachine = new AiStateMachine<IAiControlledCharacter>(this);
    }

    protected override void OnFixedUpdate(ref bool cancelUpdate)
    {
        DetectOnGround();
        AiStateMachine.Update();

        base.OnFixedUpdate(ref cancelUpdate);
    }

    protected virtual bool OnHit(Vector2 dir, float power)
    {
        return false;
    }
}