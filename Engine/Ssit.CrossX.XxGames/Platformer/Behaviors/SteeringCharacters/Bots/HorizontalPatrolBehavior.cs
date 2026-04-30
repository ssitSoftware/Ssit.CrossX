using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Steering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters.Bots;

public class HorizontalPatrolBehavior : SteeringBehavior<ISteeringCharacter>
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Parameters
    {
        internal bool MoveTowardsTarget { get; set; } = true;
        public ITarget Target{ get; set; }
        public bool WasDisplayed { get; set; }
        public Vector2 InitialPosition { get; set; }
        public float Speed { get; set; } = 10f;
    }

    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        var parameters = obj.GetParameters<Parameters>(true);
        
        if (parameters.Target is null || !parameters.WasDisplayed)
            return false;
        
        var destination = parameters.MoveTowardsTarget ? parameters.Target.Position.X : parameters.InitialPosition.X;

        if (MathF.Abs(obj.Body.Position.X - destination) < 0.1f)
        {
            parameters.MoveTowardsTarget = !parameters.MoveTowardsTarget;
            destination = parameters.MoveTowardsTarget ? parameters.Target.Position.X : parameters.InitialPosition.X;
        }

        var velocity = MathF.Sign(destination - obj.Body.Position.X) * parameters.Speed *
                       obj.Body.Simulation.SimulationParameters.TimeDelta;

        velocity = MathF.Min(velocity, MathF.Abs(destination - obj.Body.Position.X));

        obj.FaceLeft = velocity < 0;
        obj.Body.KinematicMove(new Vector2(velocity, 0), false);

        obj.SetSteeringState("Move");
        return false;
    }
}
