using System;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;

namespace Nokemono.Core.Game.Objects.Devices;

public class LaserDoorImpl : MechanicalDoor, IHittable
{
    private readonly ContextSoundContainer _soundContainer;
    
    public LaserDoorImpl(GameObjectsServices services, ObjectCreationParameters<Parameters> parameters) 
        : base(services, parameters)
    {
        InitializeSprite("assets:/Game/Objects/Laser");
        InitializePhysics(parameters, new Vector2(-0.0625f, 0), new SizeF(0.3125f, 3));
        Body.MaterialIndex = GamePhysics.Materials.Hurt;
        
        _soundContainer = services.Container.IoCConstruct<ContextSoundContainer>(new ContextSoundContainer.Parameters
        {
            Emitter = null 
        });
        _soundContainer.RegisterSound("Hit", GamePhysics.Materials.Any, "assets:/Game/Sounds/Effects/Bzzz.wav");
    }

    public void Hit(Vector2 dir, float power)
    {
        _soundContainer.Play("Hit", pitch: 0);
    }

    public bool Active => !IsOpen;
}