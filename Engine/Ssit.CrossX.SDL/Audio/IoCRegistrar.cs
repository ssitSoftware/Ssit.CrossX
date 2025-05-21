using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.IoC;
using Ssit.CrossX.SDL.Audio.Al;
using Ssit.CrossX.SDL.Audio.Dummy;

namespace Ssit.CrossX.SDL.Audio;

public static class IoCRegistrar
{
    public static IIoCContainerBuilder WithAudio(this IIoCContainerBuilder builder)
    {
        if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst() || OperatingSystem.IsWindows())
        {
            return builder.WithOpenAl();
        }
        
        return builder.WithDummyAudio();
    }
    
    private static IIoCContainerBuilder WithOpenAl(this IIoCContainerBuilder builder)
    {
        return builder
            .WithSingleton<ISoundManager, AlSoundManagerImpl>()
            .WithImplementation<ISoundEffect, AlSoundEffectImpl>()
            .WithImplementation<ISingleMusicPlayer, AlSingleMusicPlayerImpl>();
    }

    private static IIoCContainerBuilder WithDummyAudio(this IIoCContainerBuilder builder)
    {
        return builder
            .WithSingleton<ISoundManager, DummySoundManagerImpl>()
            .WithImplementation<ISoundEffect, DummySoundEffectImpl>()
            .WithImplementation<ISingleMusicPlayer, DummySingleMusicPlayerImpl>();
    }
}