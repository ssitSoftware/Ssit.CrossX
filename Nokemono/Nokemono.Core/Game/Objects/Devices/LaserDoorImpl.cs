using System;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects.Devices;

public class LaserDoorImpl : MechanicalDoor, IHittable
{
    Vector2 IHittable.Position => Body.Position;
    
    public LaserDoorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Laser");
        InitializePhysics(parameters, new Vector2(-0.0625f, 0), new SizeF(0.3125f, 3));
        Body.MaterialIndex = GamePhysics.Materials.Hurt;
    }

    public bool Hit(Vector2 dir, float power)
    {
        if (IsOpen)
            return false;
        
        Services.CommonSoundContainer.Play("Bzzz");
        return true;
    }

    protected override void SetSequence(string state)
    {
        if (state is "Opening" or "Closing")
        {
            OnAnimationFinished(state);
            return;
        }
        base.SetSequence(state);
    }
}