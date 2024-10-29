using System;
using OpenTK.Audio.OpenAL;
using Ssit.Pixel.Audio;

namespace Ssit.Pixel.NET.Audio;

internal class SoundManagerImpl: ISoundManagerInt
{
    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = value;
            MasterVolumeUpdated?.Invoke(_masterVolume);
        }
    }

    public ISoundListener SoundListener { get; set; }

    public readonly MusicPlayerImpl MusicPlayer = new();
    
    private float _masterVolume = 0.5f;
    
    public event Action<float> MasterVolumeUpdated;

    private ALDevice _device;
    private ALContext _context;
    
    public SoundManagerImpl()
    {
        _device = ALC.OpenDevice(null);

        unsafe
        {
            _context = ALC.CreateContext(_device, (int*) null);
        }

        ALC.MakeContextCurrent(_context);
    }

    public void Dispose()
    {
        MusicPlayer.Dispose();

        if (_context != ALContext.Null)
        {
            ALC.DestroyContext(_context);
            _context = ALContext.Null;
        }

        if (_device != ALDevice.Null)
        {
            ALC.CloseDevice(_device);
            _device = ALDevice.Null;
        }
    }

    
}