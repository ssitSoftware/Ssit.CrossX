using Ssit.CrossX.Games.Audio;

namespace Nokemono.Core.Game;

public static class GameInitializer
{
    public static void InitGameSounds(this ICommonSoundContainer sounds)
    {
        sounds
            .RegisterSound("RockerSwitchOn", "assets:/Game/Sounds/Devices/RockerSwitchOn.wav")
            .RegisterSound("RockerSwitchOff", "assets:/Game/Sounds/Devices/RockerSwitchOff.wav");
    }

    public static void RegisterCharacterGroundSounds(this ContextSoundContainer container)
    {
        container
            .RegisterSound("Left Foot", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultL.wav", 0.55f)
            .RegisterSound("Right Foot", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultR.wav", 0.55f)
            .RegisterSound("Jump", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultJump.wav", 0.65f)
            .RegisterSound("Land", GamePhysics.Materials.Any, "assets:/Game/Sounds/Ground/DefaultLand.wav", 0.65f)
            .RegisterSound("Left Foot", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalL.wav", 0.95f)
            .RegisterSound("Right Foot", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalR.wav", 0.95f)
            .RegisterSound("Jump", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalJump.wav", 0.95f)
            .RegisterSound("Land", GamePhysics.Materials.MetalPlatform, "assets:/Game/Sounds/Ground/MetalLand.wav", 0.95f)
            .RegisterSound("Left Foot", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterL.wav")
            .RegisterSound("Right Foot", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav")
            .RegisterSound("Jump", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterR.wav")
            .RegisterSound("Land", GamePhysics.Materials.Water, "assets:/Game/Sounds/Ground/WaterLand.wav");
    }
}