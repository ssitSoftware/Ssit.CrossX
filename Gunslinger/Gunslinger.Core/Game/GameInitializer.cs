using Ssit.CrossX.Games.Audio;

namespace Gunslinger.Core.Game;

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
            .RegisterSound("Left Foot", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultL.wav")
            .RegisterSound("Right Foot", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultR.wav")
            .RegisterSound("Jump", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultJump.wav")
            .RegisterSound("Land", GamePhysics.Materials.Any, "assets:/Sounds/Game/Ground/DefaultLand.wav")
            .RegisterSound("Left Foot", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalL.wav")
            .RegisterSound("Right Foot", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalR.wav")
            .RegisterSound("Jump", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalJump.wav")
            .RegisterSound("Land", GamePhysics.Materials.MetalPlatform, "assets:/Sounds/Game/Ground/MetalLand.wav");
    }
}