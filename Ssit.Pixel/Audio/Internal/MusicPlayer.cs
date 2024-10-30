using System;
using System.Collections.Generic;
using System.Linq;
using Ssit.Pixel.Core;
using Ssit.Pixel.IO;
using Ssit.Pixel.IoC;
using Ssit.Pixel.NET.Audio;

namespace Ssit.Pixel.Audio.Internal;

internal class MusicPlayer : MusicPlayerBase, IMusicManager, IDisposable
{
    private const int BufferLength = 88200 / 10;
    
    private readonly IFilesProvider _filesProvider;
    private readonly IEventSource _eventSource;
    private readonly IIoCContainer _iocContainer;

    private readonly List<ISingleMusicPlayer> _musicPlayers = new ();
    
    public MusicPlayer(IFilesProvider filesProvider, IEventSource eventSource, IIoCContainer iocContainer)
    {
        _filesProvider = filesProvider;
        _eventSource = eventSource;
        _iocContainer = iocContainer;

        _eventSource.Updating += EventSourceOnUpdating;
    }

    private void EventSourceOnUpdating(float dt)
    {
        foreach (var player in _musicPlayers)
        {
            player.Update(dt);
        }
    }

    public void Dispose()
    {
        foreach (var players in _musicPlayers)
        {
            players.Dispose();
        }
        _musicPlayers.Clear();
    }

    protected override int GetSongPosition()
    {
        return _musicPlayers.LastOrDefault()?.Position ?? 0;
    }

    protected override void SwitchSong(Song song, int fadeTime, int startPosition = 0)
    {
        fadeTime =  Math.Max(50, fadeTime);
        
        foreach (var player in _musicPlayers)
        {
            player.FadeOut(fadeTime);
        }

        var songStream = _filesProvider.Open(song.Path);
        var songProvider = new VorbisDataProvider(songStream);
        songProvider.Skip(startPosition, BufferLength);

        var newPlayer = _iocContainer.IoCConstruct<ISingleMusicPlayer>(this);

        _musicPlayers.Add(newPlayer);
        newPlayer.Start(songProvider, BufferLength, fadeTime);
    }

    public VorbisDataProvider GetNext()
    {
        var song = GetNextSong();
        var songStream = _filesProvider.Open(song.Path);
        return new VorbisDataProvider(songStream);
    }

    public void RemovePlayer(ISingleMusicPlayer musicPlayer)
    {
        _musicPlayers.Remove(musicPlayer);
        musicPlayer.Dispose();
    }
}