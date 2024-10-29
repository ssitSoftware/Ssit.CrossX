using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;
using OpenTK.Audio.OpenAL;
using Ssit.Pixel.Audio;
using Ssit.Pixel.Content;
using Ssit.Pixel.IoC;

namespace Ssit.Pixel.NET.Audio;

internal class SoundEffectImpl: ISoundEffect, IInstanceCountingResource
{
    private readonly IIoCContainer _iocContainer;
    private int _bufferHandle;

    private readonly List<ISoundEffectInstance> _instances = new();

    public SoundEffectImpl(IIoCContainer iocContainer, Stream stream)
    {
        _iocContainer = iocContainer;
        
        using var waveReader = new WaveFileReader(stream);
        using var wave = new WaveChannel32(waveReader);
        var sampleProvider = wave.ToSampleProvider().ToMono();
        
        
        var buffer = new float[waveReader.Length];
        
        var samplesCount = sampleProvider.Read(buffer, 0, (int)waveReader.Length);
        var shorts = new short[samplesCount];
        for (var idx = 0; idx < samplesCount; idx++)
        {
            shorts[idx] = (short)(buffer[idx] * short.MaxValue);
        }
        
        _bufferHandle = AL.GenBuffer();

        unsafe
        {
            fixed (short* ptr = shorts)
            {
                AL.BufferData(_bufferHandle, ALFormat.Mono16, (IntPtr)ptr, samplesCount * sizeof(short), 
                    sampleProvider.WaveFormat.SampleRate);
            }
        }
    }

    public ISoundEffectInstance CreateInstance() => _iocContainer.IoCConstruct<SoundEffectInstanceImpl>(this);

    public void PlayOnce(float volume = 1, float pitch = 1, ISoundEmitter emitter = null)
    {
        var instance = CreateInstance();
        instance.Emitter = emitter;
        
        _instances.Add(instance);
        
        instance.Finished += () =>
        {
            _instances.Remove(instance);
            instance.Dispose();
        };
        
        instance.Parameters = new SoundParameters
        {
            Volume = volume,
            Pitch = pitch
        };
        
        instance.Play();
    }
    
    public void Dispose()
    {
        foreach (var instance in _instances)
        {
            instance.Dispose();
        }
        _instances.Clear();
        
        if (_bufferHandle != 0)
        {
            AL.DeleteBuffer(_bufferHandle);
            _bufferHandle = 0;
        }
    }

    public Action<Guid> AddUser { get; set; }
    public Action<Guid> RemoveUser { get; set; }
    
    public int Buffer => _bufferHandle;
}