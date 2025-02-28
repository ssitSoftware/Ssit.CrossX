using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;

namespace Ssit.CrossX.SDL.Audio.Dummy;

public class DummySingleMusicPlayerImpl(IMusicDataProvider musicDataProvider, IMusicPlayer musicPlayer): ISingleMusicPlayer
{
    public void Dispose()
    {
    }

    public void Start(VorbisDataProvider provider, int bufferLength, int fadeInMilliseconds)
    {
        
    }

    public void Update(float deltaTime, out bool finished)
    {
        finished = false;
    }

    public void FadeOut(int fadeOutMilliseconds)
    {
        
    }

    public int Position => 0;
}