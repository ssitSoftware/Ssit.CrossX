namespace Ssit.CrossX.Audio.Internal;

public interface IMusicDataProvider
{
    VorbisDataProvider GetNext();
}