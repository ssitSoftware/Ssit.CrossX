using System;

namespace Ssit.Pixel.Audio.Internal;

public interface ISingleMusicPlayer: IDisposable
{
    void Start(VorbisDataProvider provider, int bufferLength, int fadeInMilliseconds);
    void Update(float deltaTime, out bool finished);
    void FadeOut(int fadeOutMilliseconds);
    int Position { get; }
}