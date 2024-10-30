using Ssit.Pixel.Audio.Internal;

namespace Ssit.Pixel.NET.Audio;

public interface IMusicManager
{
    VorbisDataProvider GetNext();
    void RemovePlayer(ISingleMusicPlayer musicPlayer);
}