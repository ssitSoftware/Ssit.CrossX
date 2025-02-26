using System;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using Ssit.CrossX.Content;

namespace Ssit.CrossX.Graphics.Sprites;

public class SpriteInstance : IDisposable
{
    public interface IHandler
    {
        void OnSpriteEvent(SpriteInstance instance, Event @event);
        void OnSequenceFinished(SpriteInstance instance, string sequenceName, bool reverse);
    }
    
    public class Event(string sequenceName, int frame, string eventName, string parameters)
    {
        internal readonly int Frame = frame;
        
        public string EventName { get; } = eventName;
        public string SequenceName { get; } = sequenceName;

        public TParameters GetParameters<TParameters>() where TParameters: class, new() => JsonConvert.DeserializeObject<TParameters>(parameters);
    }
    
    private readonly Vector2 _origin;

    public delegate void SpriteEventDelegate(SpriteInstance instance, Event @event);
    public delegate void SequenceFinishedDelegate(SpriteInstance instance, string sequenceName, bool reverse);

    private readonly ResourceHandle<Sprite> _sprite;
    private readonly ResourceHandle<ITexture> _spriteSheet;
    
    public ITexture SpriteSheet => _spriteSheet.Resource;
    
    public Vector2 Origin { get; private set; }
    public RectangleF Source { get; private set; }
    public string CurrentSequence => _currentSequence?.Name ?? "";

    private float _currentTime;
    private float _maxTime;
    private int _lastFrame = -1;
    private float _maxTimeDelta = 0.1f;

    private Sprite.SpriteSequence _currentSequence;

    private readonly Dictionary<(string, int), Event> _events;

    public IHandler Handler { get; set; }
    
    public SpriteInstance(string spritePath, Vector2 origin, IReadOnlyList<Event> events, IContentManager contentManager)
    {
        _sprite = contentManager.Get<Sprite>(spritePath);
        _spriteSheet = contentManager.Get<ITexture>(_sprite.Resource.SheetName);

        _origin = origin;
        _events = PrepareEvents(events);
    }

    private Dictionary<(string, int), Event> PrepareEvents(IReadOnlyList<Event> events)
    {
        if(events is null) return null;
        if(events.Count == 0) return null;
        
        var dict = new Dictionary<(string, int), Event>();
        
        foreach (var @event in events)
        {
            dict.Add((@event.SequenceName, @event.Frame), @event);
        }

        return dict;
    }

    public void SetSequence(string sequenceName, bool resetPosition = false)
    {
        if (_currentSequence?.Name == sequenceName)
        {
            if (resetPosition)
            {
                _currentTime = 0;
                _lastFrame = -1;
            }
            return;
        }

        _currentSequence = _sprite.Resource.GetSequence(sequenceName);
        
        _currentTime = 0;
        _lastFrame = -1;
        _maxTime = 0;
        _maxTimeDelta = 0.5f;

        foreach (var frame in _currentSequence.Frames)
        {
            _maxTime += frame.Duration;
            _maxTimeDelta = MathF.Min(_maxTimeDelta, frame.Duration);
        }
        
        AdvanceOne(0, false);

        Source = _currentSequence.Frames[_lastFrame].Source;
        Origin = _currentSequence.Frames[_lastFrame].Offset + _origin;
    }

    public void Advance(float dt, bool reverse = false)
    {
        if (_currentSequence is null)
        {
            Source = Rectangle.Empty;
            Origin = Vector2.Zero;
            return;
        }

        while (dt > 0)
        {
            var delta = MathF.Min(dt, _maxTimeDelta);
            dt -= delta;
            AdvanceOne(delta, reverse);
        }

        Source = _currentSequence.Frames[_lastFrame].Source;
        Origin = _currentSequence.Frames[_lastFrame].Offset + _origin;
    }

    private void AdvanceOne(float delta, bool reverse)
    {
        _currentTime += delta * (reverse ? -1 : 1);
        var currentSequence = _currentSequence.Name;

        if (_currentTime >= _maxTime || _currentTime < 0)
        {
            Handler?.OnSequenceFinished(this, _currentSequence.Name, _currentTime < 0);
            if (_currentSequence?.Name != currentSequence || _currentSequence is null)
            {
                return;
            }
        }

        _currentTime += _maxTime;
        _currentTime %= _maxTime;

        var time = _currentTime;
        var frame = 0;

        while (time >= _currentSequence.Frames[frame].Duration)
        {
            time -= _currentSequence.Frames[frame].Duration;
            frame++;
        }

        if (frame != _lastFrame)
        {
            if(_events != null && _events.TryGetValue((currentSequence, frame), out var @event))
            {
                Handler?.OnSpriteEvent(this, @event);
            }
        }

        _lastFrame = frame;
    }

    public void Dispose()
    {
        _spriteSheet?.Dispose();
        _sprite?.Dispose();
    }
}