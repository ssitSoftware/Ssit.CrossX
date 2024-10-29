using System;
using Ssit.Pixel.Audio;

namespace Ssit.Pixel.NET.Audio;

internal class MusicPlayerImpl : IMusicPlayer, IDisposable
{
    public void RegisterPlaylist(string name, MusicPlaylist playlist)
    {
        //throw new NotImplementedException();
    }

    public void ChangePlaylist(string name, int fadeTimeMs = 0, bool resetProgress = false)
    {
        //throw new NotImplementedException();
    }

    public bool NextTrack(int fadeTimeMs = 0, bool loop = true)
    {
        return false;
    }

    public bool PreviousTrack(int fadeTimeMs = 0, bool loop = true)
    {
        return false;
    }

    public void Dispose()
    {
        //SDL_mixer.Mix_HaltMusic();
        //Mix_FreeMusic();
    }
}