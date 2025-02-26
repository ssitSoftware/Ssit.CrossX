using OpenTK.Audio.OpenAL;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Audio.Internal;

namespace Ssit.CrossX.SDL3.Audio;

internal class SingleMusicPlayerImpl: ISingleMusicPlayer
{
    public enum Mode
    {
        FadeIn,
        FadeOut,
        Normal
    }

    public int Position { get; private set; }
    
    private VorbisDataProvider _dataProvider;
    private readonly IMusicDataProvider _musicDataProvider;
    private readonly IMusicPlayer _musicPlayer;
    private int _alSource;

    private int[] _buffers;
    private short[] _dataBuffer;

    private float _fadeSpeed;
    private float _volume;
    private Mode _mode;
    
    public SingleMusicPlayerImpl(IMusicDataProvider musicDataProvider, IMusicPlayer musicPlayer)
    {
        _musicDataProvider = musicDataProvider;
        _musicPlayer = musicPlayer;
    }
    
    public void Start(VorbisDataProvider provider, int bufferLength, int fadeInMilliseconds)
    {
        _dataProvider = provider;
        _alSource = AL.GenSource();

        _buffers = AL.GenBuffers(8);
        _dataBuffer = new short[bufferLength];
        
        _fadeSpeed = 1000f / fadeInMilliseconds;
        _mode = Mode.FadeIn;
        _volume = 0;
        UpdateVolume();

        Start();
    }

    private void UpdateVolume()
    {
        AL.Source(_alSource, ALSourcef.Gain, _volume * _musicPlayer.Volume);
    }

    private void Start()
    {
        for (var idx = 0; idx < _buffers.Length; idx++)
        {
            FillBuffer(_buffers[idx]);
        }
        AL.SourceQueueBuffers(_alSource, _buffers);
        AL.SourcePlay(_alSource);
    }

    private bool FillBuffer(int bufferId)
    {
        if (_dataProvider is null) return false;
        
        var dataRead = _dataProvider.Read(_dataBuffer);
        if (dataRead == 0) return false;
        
        unsafe
        {
            fixed (void* ptr = _dataBuffer)
            {
                AL.BufferData(bufferId, ALFormat.Stereo16, (IntPtr)ptr, dataRead * sizeof(short), _dataProvider.Frequency);
            }
        }
        
        return true;
    }

    public void FadeOut(int fadeOutMilliseconds)
    {
        if (_mode == Mode.FadeOut)
            return;
        
        _fadeSpeed = 1000f / fadeOutMilliseconds;
        _mode = Mode.FadeOut;
    }
    
    public void Update(float dt, out bool finished)
    {
        finished = false;
        switch (_mode)
        {
            case Mode.FadeIn:
                _volume += _fadeSpeed * dt;
                if (_volume >= 1)
                {
                    _volume = 1;
                    _mode = Mode.Normal;
                }
                break;
            
            case Mode.FadeOut:
                _volume -= _fadeSpeed * dt;
                if (_volume <= 0)
                {
                    _volume = 0;
                    finished = true;
                    return;
                }
                break;
        }
        
        UpdateVolume();
        
        var state = (ALSourceState) AL.GetSource(_alSource, ALGetSourcei.SourceState);
        var buffersProcessed = AL.GetSource(_alSource, ALGetSourcei.BuffersProcessed);
        
        while (buffersProcessed > 0)
        {
            Position++;
            buffersProcessed--;
            
            var buf = AL.SourceUnqueueBuffer(_alSource);
            if (!FillBuffer(buf))
            {
                if (_mode == Mode.FadeOut)
                {
                    _dataProvider?.Dispose();
                    _dataProvider = null;
                    continue;
                }
                
                _dataProvider.Dispose();
                _dataProvider = _musicDataProvider.GetNext();

                var buffersQueued = AL.GetSource(_alSource, ALGetSourcei.BuffersQueued);
                Position = -buffersQueued;
                
                if (!FillBuffer(buf))
                {
                    throw new InvalidProgramException();
                }
            }

            AL.SourceQueueBuffer(_alSource, buf);
        }

        if (state == ALSourceState.Stopped)
        {
            AL.SourcePlay(_alSource);
        }
    }
    
    public void Dispose()
    {
        _dataProvider?.Dispose();
        _dataProvider = null;
        AL.SourceStop(_alSource);
        AL.DeleteBuffers(_buffers);
        AL.DeleteSource(_alSource);
    }
}