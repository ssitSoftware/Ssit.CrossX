using System;
using Ssit.CrossX.Audio;
using OpenTK.Audio.OpenAL;
using Ssit.CrossX.Core;

namespace Ssit.CrossX.NET.Audio;

internal class SoundEffectInstanceImpl: ISoundEffectInstance
{
    public readonly Guid Guid = Guid.NewGuid();
    private readonly SoundEffectImpl _soundEffect;
    private readonly IEventSource _eventSource;
    private readonly ISoundManager _soundManager;

    private SoundParameters _parameters;
    private int _sourceHandle = 0;

    public SoundEffectInstanceImpl(SoundEffectImpl soundEffect, IEventSource eventSource, ISoundManager soundManager)
    {
        _soundEffect = soundEffect;
        _eventSource = eventSource;
        _soundManager = soundManager;
        soundEffect.AddUser?.Invoke(Guid);
    }

    private void CheckPlaying(float _)
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
        
        if (_emitter is not null)
        {
            _emitter.ParametersUpdated -= UpdateParameters;
            _emitter = null;
        }
        
        _soundEffect.RemoveUser?.Invoke(Guid);
    }

    public event Action Finished;

    public ISoundEmitter Emitter
    {
        get => _emitter;
        set
        {
            if (_emitter is not null)
            {
                _emitter.ParametersUpdated -= UpdateParameters;
            }
            
            _emitter = value;

            if (_emitter is not null)
            {
                UpdateParameters();
                _emitter.ParametersUpdated += UpdateParameters;
            }
        }
    }

    public bool IsPlaying => _sourceHandle != 0 && !_paused;

    private bool _paused;
    private ISoundEmitter _emitter;

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

    private void UpdateVolume()
    {
        if (_sourceHandle == 0)
        {
            return;
        }
        
        var volume = _parameters.Volume * _soundManager.MasterVolume;
        volume = MathF.Max(0, MathF.Min(1, volume));
        AL.Source(_sourceHandle, ALSourcef.Gain, volume);
    }
    
    private void UpdateParameters()
    {
        if (_sourceHandle == 0)
        {
            return;
        }

        UpdateVolume();
        
        var pitch = _parameters.Pitch;
        pitch = MathF.Max(0, MathF.Min(4, pitch));
        
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
        _soundManager.MasterVolumeUpdated += UpdateVolume;
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
        _soundManager.MasterVolumeUpdated -= UpdateVolume;
        _eventSource.Updating -= CheckPlaying;
        
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