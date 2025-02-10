using System.Numerics;
using OpenTK.Audio.OpenAL;
using Ssit.CrossX.Audio;

namespace Ssit.CrossX.SDL3.Audio;

internal class SoundManagerImpl: ISoundManager
{
    private readonly float[] _listenerOrientation = new float[6];

    public event Action MasterVolumeUpdated;

    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = value;
            MasterVolumeUpdated?.Invoke();
        }
    }

    public ISoundListener SoundListener
    {
        get => _soundListener;
        set
        {
            if (_soundListener is not null)
            {
                _soundListener.ParametersUpdated -= UpdateListener;
            }
            
            _soundListener = value;
            
            if (_soundListener is not null)
            {
                _soundListener.ParametersUpdated += UpdateListener;
            }
            UpdateListener();
        }
    }
    
    private float _masterVolume = 0.5f;

    private ALDevice _device;
    private ALContext _context;
    private ISoundListener _soundListener;

    public SoundManagerImpl()
    {
        _device = ALC.OpenDevice(null);

        unsafe
        {
            _context = ALC.CreateContext(_device, (int*) null);
        }

        ALC.MakeContextCurrent(_context);
        UpdateListener();
    }

    private void UpdateListener()
    {
        var position = _soundListener?.Position ?? new Vector3(0, 0, -2);
        var velocity = _soundListener?.Velocity ?? Vector3.Zero;
        var at = _soundListener?.At ?? new Vector3(0, 0, 0);
        var up = _soundListener?.Up ?? new Vector3(0, 1, 0);

        _listenerOrientation[0] = at.X;
        _listenerOrientation[1] = at.Y;
        _listenerOrientation[2] = at.Z;
        
        _listenerOrientation[3] = up.X;
        _listenerOrientation[4] = up.Y;
        _listenerOrientation[5] = up.Z;
        
        AL.Listener(ALListener3f.Position, position.X, position.Y, position.Z);
        AL.Listener(ALListener3f.Velocity, velocity.X, velocity.Y, velocity.Z);
        AL.Listener(ALListenerfv.Orientation, _listenerOrientation);
        AL.Listener(ALListenerf.Gain, 1);
    }
    
    public void Dispose()
    {
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