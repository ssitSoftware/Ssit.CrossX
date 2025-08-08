using Ssit.CrossX.Games.Audio;

namespace Nokemono.Core.Game;

public static class GameInitializer
{
    public static void InitGameSounds(this ICommonSoundContainer sounds)
    {
        sounds
            .RegisterSound("RockerSwitchOn", "assets:/Game/Sounds/Devices/RockerSwitchOn.wav")
            .RegisterSound("RockerSwitchOff", "assets:/Game/Sounds/Devices/RockerSwitchOff.wav")
            .RegisterSound("Bzzz", "assets:/Game/Sounds/Effects/Bzzz.wav")
            .RegisterSound("WoodBreak", "assets:/Game/Sounds/Effects/WoodBreak.wav")
            .RegisterSound("SwordFlesh", "assets:/Game/Sounds/Effects/SwordIntoFlesh.wav");
    }

    public static void RegisterCharacterGroundSounds(this ContextSoundContainer container)
    {
        container
            .RegisterSound("Left Foot", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultL.wav", 0.55f)
            .RegisterSound("Right Foot", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultR.wav", 0.55f)
            .RegisterSound("Left Foot Silent", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultL.wav", 0.16f)
            .RegisterSound("Right Foot Silent", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultR.wav", 0.16f)
            .RegisterSound("Jump", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultJump.wav", 0.65f)
            .RegisterSound("Land", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultLand.wav", 0.65f)
            .RegisterSound("Left Foot", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalL.wav", 0.95f)
            .RegisterSound("Right Foot", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalR.wav", 0.95f)
            .RegisterSound("Left Foot Silent", GamePhysics.Materials.MetalPlatform,  "assets:/Game/Sounds/Ground/MetalL.wav", 0.3f)
            .RegisterSound("Right Foot Silent", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalR.wav", 0.3f)
            .RegisterSound("Jump", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalJump.wav", 0.95f)
            .RegisterSound("Land", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalLand.wav", 0.95f)
            .RegisterSound("Left Foot", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterL.wav")
            .RegisterSound("Right Foot", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav")
            .RegisterSound("Left Foot Silent", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterL.wav", 0.3f)
            .RegisterSound("Right Foot Silent", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav", 0.3f)
            .RegisterSound("Jump", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav")
            .RegisterSound("Land", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterLand.wav");
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