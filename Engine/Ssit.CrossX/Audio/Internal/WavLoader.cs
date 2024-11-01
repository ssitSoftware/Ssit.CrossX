using System;
using System.IO;
using NAudio.Wave;

namespace Ssit.CrossX.Audio.Internal;

public static class WavLoader
{
    public static (short[] buffer, int sampleRate) LoadMonoWav(Stream stream)
    {
        using var waveReader = new WaveFileReader(stream);
        using var wave = new WaveChannel32(waveReader);
        wave.PadWithZeroes = false;
        
        var stereoSampleLength = wave.Length / sizeof(float);
        var sampleProvider = wave.ToSampleProvider().ToMono();

        var monoLength = stereoSampleLength / 2;
        var buffer = new float[monoLength + 256];
        
        var size = sampleProvider.Read(buffer, 0, buffer.Length);
        
        var shorts = new short[size];
        for (var idx = 0; idx < size; idx++)
        {
            shorts[idx] = (short)(buffer[idx] * short.MaxValue);
        }
        
        return (shorts, sampleProvider.WaveFormat.SampleRate);
    }
}