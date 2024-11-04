using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;
using Ssit.CrossX.IoC;

namespace Ssit.CrossX.NET.Audio;

public static class IoCRegistrar
{
    public static IIoCContainerBuilder WithOpenAl(this IIoCContainerBuilder builder)
    {
        return builder
            .WithSingleton<ISoundManager, SoundManagerImpl>()
            .WithImplementation<ISingleMusicPlayer, SingleMusicPlayerImpl>();
    }
}