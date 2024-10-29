using System.Collections;
using System.Collections.Generic;

namespace Ssit.Pixel.Audio;

public interface IPlaylist
{
    int CurrentPosition { get; }

    /// <summary>
    /// Gets or sets the index of the currently playing song in the playlist.
    /// </summary>
    int CurrentSong { get; }

    /// <summary>
    /// Gets a read-only list of songs in the music playlist.
    /// </summary>
    IReadOnlyList<Song> List { get; }
}

/// <summary>
/// Represents a music playlist that holds a collection of songs and provides functionality to manage them.
/// </summary>
public class MusicPlaylist: IEnumerable<Song>, IPlaylist
{
    /// <summary>
    /// Represents the list of songs in the music playlist.
    /// </summary>
    private readonly List<Song> _playlist = new();

    /// <summary>
    /// Gets or sets the current position in milliseconds of the song being played in the playlist.
    /// </summary>
    internal int CurrentPosition { get; set; }

    /// <summary>
    /// Gets or sets the index of the currently playing song in the playlist.
    /// </summary>
    internal int CurrentSong { get; set; }

    /// <summary>
    /// Gets a read-only list of songs in the music playlist.
    /// </summary>
    internal IReadOnlyList<Song> List => _playlist;

    /// <summary>
    /// Adds the specified song to the playlist.
    /// </summary>
    /// <param name="song">The song to add to the playlist.</param>
    public void Add(Song song) => _playlist.Add(song);

    /// <summary>
    /// Returns an enumerator that iterates through the playlist.
    /// </summary>
    /// <returns>An enumerator for the playlist.</returns>
    IEnumerator<Song> IEnumerable<Song>.GetEnumerator() => _playlist.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the MusicPlaylist.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the MusicPlaylist.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => _playlist.GetEnumerator();
    
    IReadOnlyList<Song> IPlaylist.List => _playlist;
    int IPlaylist.CurrentSong => CurrentSong;
    int IPlaylist.CurrentPosition => CurrentPosition;
}