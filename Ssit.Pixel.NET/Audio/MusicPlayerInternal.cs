using System;
using OpenTK.Audio.OpenAL;
using Ssit.Pixel.Audio.Internal;

namespace Ssit.Pixel.NET.Audio;

internal class MusicPlayerInternal: IDisposable
{
    public int Position { get; private set; } = 0;
    
    private readonly VorbisDataProvider _dataProvider;
    private readonly int _alSource;

    private readonly int[] _buffers;
    private readonly short[] _dataBuffer;
    
    public float Volume
    {
        set => AL.Source(_alSource, ALSourcef.Gain, value);
    }
    
    public MusicPlayerInternal(VorbisDataProvider dataProvider, int bufferSize)
    {
        _dataProvider = dataProvider;
        
        _alSource = AL.GenSource();

        _buffers = AL.GenBuffers(8);
        _dataBuffer = new short[bufferSize];

        Start();
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
    
    public bool Update()
    {
        var state = (ALSourceState) AL.GetSource(_alSource, ALGetSourcei.SourceState);
        var buffersProcessed = AL.GetSource(_alSource, ALGetSourcei.BuffersProcessed);
        
        var firstBuffer = true;
        while (buffersProcessed > 0)
        {
            Position++;
            buffersProcessed--;
            
            var buf = AL.SourceUnqueueBuffer(_alSource);
            if (FillBuffer(buf))
            {
                AL.SourceQueueBuffer(_alSource, buf);
            }
            else if (firstBuffer)
            {
                return false;
            }
            
            firstBuffer = false;
        }

        if (state == ALSourceState.Stopped)
        {
            AL.SourcePlay(_alSource);
        }

        return true;
    }
    
    public void Dispose()
    {
        AL.SourceStop(_alSource);
        AL.DeleteBuffers(_buffers);
        AL.DeleteSource(_alSource);
    }
}