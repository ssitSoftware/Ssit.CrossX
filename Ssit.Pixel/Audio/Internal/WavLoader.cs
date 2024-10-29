using System;
using System.IO;
using NAudio.Wave;

namespace Ssit.Pixel.Audio.Internal;

public static class WavLoader
{
    public static (short[] buffer, int sampleRate) LoadMonoWav(Stream stream)
    {
        using var waveReader = new WaveFileReader(stream);
        using var wave = new WaveChannel32(waveReader);
        
        var sampleProvider = wave.ToSampleProvider().ToMono();
        
        var buffer = new float[waveReader.Length * 2];
        sampleProvider.Read(buffer, 0, buffer.Length);
        
        var size = 0;
        float minValue = 0.75f / short.MaxValue;
        
        for (var idx = 0; idx < buffer.Length; idx++)
        {
            if (MathF.Abs(buffer[idx]) > minValue)
            {
                size = idx;
            }
        }
        
        var shorts = new short[size];
        
        for (var idx = 0; idx < size; idx++)
        {
            shorts[idx] = (short)(buffer[idx] * short.MaxValue);
        }
        
        return (shorts, sampleProvider.WaveFormat.SampleRate);
    }
}