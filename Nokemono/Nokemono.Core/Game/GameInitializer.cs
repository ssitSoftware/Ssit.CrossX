using Ssit.CrossX;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Rendering;

namespace Nokemono.Core.Game;

public static class GameConstants
{
    public const int BloodParticles = 1;
    public const int ShredsParticles = 2;
    public const int DustParticles = 3;
}

public static class GameInitializer
{
    public static void InitGameParticles(this IParticleSystem particleSystem)
    {
        particleSystem
            .RegisterParticleGroup(GameConstants.BloodParticles, "assets:/Game/Objects/Blood.png", new Size(8,8), 0.25f)
            .RegisterParticleGroup(GameConstants.ShredsParticles, "assets:/Game/Objects/Shred.png", new Size(8,8), 1)
            .RegisterParticleGroup(GameConstants.DustParticles, "assets:/Game/Objects/Dust.png", new Size(3,3), 1f);
    }
    
    public static void InitGameSounds(this ICommonSoundContainer sounds)
    {
        sounds
            .RegisterSound("RockerSwitchOn", "assets:/Game/Sounds/Devices/RockerSwitchOn.wav")
            .RegisterSound("RockerSwitchOff", "assets:/Game/Sounds/Devices/RockerSwitchOff.wav")
            .RegisterSound("Bzzz", "assets:/Game/Sounds/Effects/Bzzz.wav")
            .RegisterSound("WoodBreak", "assets:/Game/Sounds/Effects/WoodBreak.wav")
            .RegisterSound("SwordFlesh", "assets:/Game/Sounds/Effects/SwordIntoFlesh.wav")
            .RegisterSound("HitDummy", "assets:/Game/Sounds/Effects/Thump.wav");
    }

    public static void RegisterCharacterGroundSounds(this ContextSoundContainer container)
    {
        container
            .RegisterSound("Left Foot", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultL.wav", 0.45f)
            .RegisterSound("Right Foot", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultR.wav", 0.45f)
            .RegisterSound("Left Foot Silent", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultL.wav", 0.16f)
            .RegisterSound("Right Foot Silent", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultR.wav", 0.16f)
            .RegisterSound("Jump", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultJump.wav", 0.55f)
            .RegisterSound("Land", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultLand.wav", 0.65f)
            .RegisterSound("Left Foot", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalL.wav", 0.85f)
            .RegisterSound("Right Foot", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalR.wav", 0.85f)
            .RegisterSound("Left Foot Silent", GamePhysics.Materials.MetalPlatform,  "assets:/Game/Sounds/Ground/MetalL.wav", 0.3f)
            .RegisterSound("Right Foot Silent", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalR.wav", 0.3f)
            .RegisterSound("Jump", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalJump.wav", 0.85f)
            .RegisterSound("Land", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalLand.wav", 0.95f)
            .RegisterSound("Left Foot", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterL.wav", 0.8f)
            .RegisterSound("Right Foot", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav", 0.8f)
            .RegisterSound("Left Foot Silent", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterL.wav", 0.2f)
            .RegisterSound("Right Foot Silent", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav", 0.2f)
            .RegisterSound("Jump", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav", 0.9f)
            .RegisterSound("Land", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterLand.wav", 0.9f);
    }
    
    public static void RegisterCharacterEffectSounds(this ContextSoundContainer container)
    {
        container
            .RegisterSound("Slash 1", GamePhysics.Materials.Any, "assets:/Game/Sounds/Effects/Slash1.wav")
            .RegisterSound("Slash 2", GamePhysics.Materials.Any, "assets:/Game/Sounds/Effects/Slash2.wav")
            .RegisterSound("Spin Attack", GamePhysics.Materials.Any, "assets:/Game/Sounds/Effects/Slash3.wav")
            .RegisterSound("Slam", GamePhysics.Materials.Any, "assets:/Game/Sounds/Effects/Slam.wav");
    }
}