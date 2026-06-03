using System;
using System.Numerics;
using Ssit.CrossX.XxGames.Logic.Objects.Characters;
using Ssit.CrossX.XxGames.Logic.Stering;

namespace Ssit.CrossX.XxGames.Platformer.Behaviors.SteeringCharacters.Bots;

public class VerticalPatrolBehavior(bool returnToOriginalPositionIfNotVisible) : SteeringBehavior<ISteeringCharacter>
{
    protected override bool OnFixedUpdate(ISteeringCharacter obj, float dt)
    {
        var parameters = obj.GetParameters<HorizontalPatrolBehavior.Parameters>(true);
        parameters.WasInitialized |= parameters.WasDisplayed;

        if (parameters.Target is null || !parameters.WasInitialized)
            return false;

        bool moveTowardsTarget = parameters.MoveTowardsTarget;

        if (!parameters.WasDisplayed && returnToOriginalPositionIfNotVisible)
        {
            moveTowardsTarget = false;
        }

        var destination = moveTowardsTarget ? parameters.Target.Position.Y : parameters.InitialPosition.Y;

        if (MathF.Abs(obj.Body.Position.Y - destination) < 0.1f)
        {
            parameters.MoveTowardsTarget = !moveTowardsTarget;
            destination = parameters.MoveTowardsTarget ? parameters.Target.Position.Y : parameters.InitialPosition.Y;
        }

        var velocity = MathF.Sign(destination - obj.Body.Position.Y) * parameters.Speed *
                       obj.Body.Simulation.SimulationParameters.TimeDelta;

        velocity = MathF.Min(velocity, MathF.Abs(destination - obj.Body.Position.Y));

        obj.Body.KinematicMove(new Vector2(0, velocity), false);

        obj.SetSteeringState("Move");
        return false;
    }
}
