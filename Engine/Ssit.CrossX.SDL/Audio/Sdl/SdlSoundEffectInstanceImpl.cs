using SDL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Core;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3_mixer;

namespace Ssit.CrossX.SDL.Audio.Sdl;

public unsafe class SdlSoundEffectInstanceImpl : ISoundEffectInstance
{
    public event Action<ISoundEffectInstance> Finished;
    public ISoundEmitter Emitter { get; set; }
    public SoundParameters Parameters { get; set; }

    public bool IsPlaying
    {
        get
        {
            if (_currentChannel < 0)
            {
                return false;
            }
            
            return Mix_Playing(_currentChannel) > 0;
        }
    }

    private int _currentChannel = -1;
    private readonly SdlSoundManagerImpl _sm;
    private readonly SdlHandle<Mix_Chunk> _chunk;
    private readonly IEventSource _eventSource;

    public SdlSoundEffectInstanceImpl(ISoundManager sm, SdlHandle<Mix_Chunk> chunk, IEventSource eventSource)
    {
        _sm = (SdlSoundManagerImpl)sm;
        _chunk = chunk;
        _eventSource = eventSource;
        _eventSource.Updating += CheckPlaying;
        _sm.ChannelIntercepted += SmOnChannelFinished;
    }

    private void CheckPlaying(float _)
    {
        if (_currentChannel >= 0)
        {
            if (Mix_Playing(_currentChannel) == 0)
            {
                _currentChannel = -1;
                Finished?.Invoke(this);
            }
        }
    }

    private void SmOnChannelFinished(int channel)
    {
        if (_currentChannel == channel)
        {
            _currentChannel = -1;
            Finished?.Invoke(this);
        }
    }

    public void Dispose()
    {
        _sm.ChannelIntercepted -= SmOnChannelFinished;
        _eventSource.Updating -= CheckPlaying;
        
        if (_currentChannel >= 0)
        {
            Mix_HaltChannel(_currentChannel);
            _currentChannel = -1;
        }
    }
    
    public void Play(bool loop = false)
    {
        if (_currentChannel < 0)
        {
            var channel = Mix_PlayChannel(-1, _chunk.Pointer, loop ? 1 : 0);
            Mix_Volume(channel, (int) (Parameters.Volume * 128));

            _sm.OnChannelIntercepted(channel);
            
            _currentChannel = channel;
        }
    }

    public void Stop()
    {
        if (_currentChannel >= 0)
        {
            Mix_HaltChannel(_currentChannel);
            _currentChannel = -1;
        }
    }

    public void Pause()
    {
        if (_currentChannel >= 0)
        {
            Mix_Pause(_currentChannel);
        }
    }

    public void Resume()
    {
        if (_currentChannel >= 0)
        {
            Mix_Resume(_currentChannel);
        }
    }
}