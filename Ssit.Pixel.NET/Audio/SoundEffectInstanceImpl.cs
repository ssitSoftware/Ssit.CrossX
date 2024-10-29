using System;
using Ssit.Pixel.Audio;
using OpenTK.Audio.OpenAL;
using Ssit.Pixel.Core;


namespace Ssit.Pixel.NET.Audio;

internal class SoundEffectInstanceImpl: ISoundEffectInstance
{
    public readonly Guid Guid = Guid.NewGuid();
    private readonly SoundEffectImpl _soundEffect;
    private readonly ISoundManagerInt _soundManager;
    private readonly IEventSource _eventSource;

    private SoundParameters _parameters;
    private int _sourceHandle = 0;

    public SoundEffectInstanceImpl(SoundEffectImpl soundEffect, ISoundManagerInt soundManager, IEventSource eventSource )
    {
        _soundEffect = soundEffect;
        _soundManager = soundManager;
        _eventSource = eventSource;
        soundEffect.AddUser?.Invoke(Guid);
    }

    private void CheckPlaying()
    {
        if (_sourceHandle == 0)
        {
            return;
        }
        
        var state = (ALSourceState)AL.GetSource(_sourceHandle, ALGetSourcei.SourceState);
        if (state is ALSourceState.Stopped)
        {
            CleanUpSource();
            Finished?.Invoke();
        }
    }

    public void Dispose()
    {
        CleanUpSource();
        _soundEffect.RemoveUser?.Invoke(Guid);
    }

    private void OnMasterVolumeUpdated(float _)
    {
        UpdateParameters();
    }

    public event Action Finished;
    public ISoundEmitter Emitter { get; set; }

    public bool IsPlaying => _sourceHandle != 0 && !_paused;

    private bool _paused;
    
    public SoundParameters Parameters
    {
        get => _parameters;
        set
        {
            _parameters = value;
            if (IsPlaying)
            {
                UpdateParameters();
            }
        }
    }

    private void UpdateParameters()
    {
        if (_sourceHandle == 0)
        {
            return;
        }

        var volume = _parameters.Volume * _soundManager.MasterVolume;
        var pitch = _parameters.Pitch;

        volume = MathF.Max(0, MathF.Min(1, volume));
        pitch = MathF.Max(0, MathF.Min(4, pitch));

        AL.Source(_sourceHandle, ALSourcef.Gain, volume);
        AL.Source(_sourceHandle, ALSourcef.Pitch, pitch);

        if (Emitter is not null)
        {
            
            var vec3 = Emitter.Position;
            AL.Source(_sourceHandle, ALSource3f.Position, vec3.X, vec3.Y, vec3.Z);

            vec3 = Emitter.Velocity;
            AL.Source(_sourceHandle, ALSource3f.Velocity, vec3.X, vec3.Y, vec3.Z);

            vec3 = Emitter.Direction;
            AL.Source(_sourceHandle, ALSource3f.Direction, vec3.X, vec3.Y, vec3.Z);
        }
    }

    public void Play(bool loop = false)
    {
        if (_sourceHandle != 0)
        {
            AL.Source(_sourceHandle, ALSourceb.Looping, loop);
            return;
        }
        
        _paused = false;
        
        _eventSource.Updating += CheckPlaying;
        _soundManager.MasterVolumeUpdated += OnMasterVolumeUpdated;
        
        _sourceHandle = AL.GenSource();
        UpdateParameters();
        
        AL.Source(_sourceHandle, ALSourceb.Looping, loop);
        AL.SourceQueueBuffer(_sourceHandle, _soundEffect.Buffer);
        AL.SourcePlay(_sourceHandle);
    }

    public void Stop()
    {
        if (_sourceHandle != 0)
        {
            CleanUpSource();
            Finished?.Invoke();
        }
    }

    private void CleanUpSource()
    {
        _eventSource.Updating -= CheckPlaying;
        _soundManager.MasterVolumeUpdated -= OnMasterVolumeUpdated;
        
        if(_sourceHandle != 0)
        {
            AL.SourceStop(_sourceHandle);
            AL.DeleteSource(_sourceHandle);
            _sourceHandle = 0;
        }
    }

    public void Pause()
    {
        if (_sourceHandle != 0)
        {
            AL.SourcePause(_sourceHandle);
            _paused = true;
        }
    }

    public void Resume()
    {
        if (_sourceHandle >= 0)
        {
            _paused = false;
            UpdateParameters();
            AL.SourcePlay(_sourceHandle);
        }
    }
}