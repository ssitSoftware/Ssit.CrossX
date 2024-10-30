namespace Ssit.Pixel.Audio.Internal;

public interface IMusicDataProvider
{
    VorbisDataProvider GetNext();
}