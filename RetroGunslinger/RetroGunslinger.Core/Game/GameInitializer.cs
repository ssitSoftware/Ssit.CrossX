using Ssit.CrossX.Games.Audio;

namespace RetroGunslinger.Core.Game;

public static class GameInitializer
{
    public static void InitGameSounds(this ICommonSoundContainer sounds)
    {
        sounds
            .RegisterSound("RockerSwitchOn", "assets:/Sounds/Game/Devices/RockerSwitchOn.wav")
            .RegisterSound("RockerSwitchOff", "assets:/Sounds/Game/Devices/RockerSwitchOff.wav");
    }

    public static void RegisterCharacterGroundSounds(this ContextSoundContainer container)
    {
        container
            .RegisterSound("Left Foot", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultL.wav", 0.55f)
            .RegisterSound("Right Foot", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultR.wav", 0.55f)
            .RegisterSound("Jump", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultJump.wav", 0.65f)
            .RegisterSound("Land", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultLand.wav", 0.65f)
            .RegisterSound("Left Foot", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalL.wav", 0.95f)
            .RegisterSound("Right Foot", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalR.wav", 0.95f)
            .RegisterSound("Jump", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalJump.wav", 0.95f)
            .RegisterSound("Land", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalLand.wav", 0.95f)
            .RegisterSound("Left Foot", GamePhysics.Materials.Water, "assets:/Sounds/Game/Ground/WaterL.wav")
            .RegisterSound("Right Foot", GamePhysics.Materials.Water, "assets:/Sounds/Game/Ground/WaterR.wav")
            .RegisterSound("Jump", GamePhysics.Materials.Water, "assets:/Sounds/Game/Ground/WaterR.wav")
            .RegisterSound("Land", GamePhysics.Materials.Water, "assets:/Sounds/Game/Ground/WaterLand.wav");
    }
}