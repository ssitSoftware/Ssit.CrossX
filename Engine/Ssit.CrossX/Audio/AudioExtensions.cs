using Ssit.IoC;

namespace Ssit.CrossX.Audio;

public static class AudioExtensions
{
    public static IIoCContainerBuilder WithCommonSoundsContainer(this IIoCContainerBuilder builder)
    {
        return builder.WithSingleton<ICommonSoundContainer, CommonSoundContainer>();
    }
}