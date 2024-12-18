using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Input.Internal;

public class TouchProcessor
{
    public enum TouchEventKind
    {
        Down,
        Move,
        Up,
        Cancel
    }

    private struct TouchEvent
    {
        public TouchEventKind Kind;
        public int Id;
        public Vector2 Position;
        public double InitialTime;
        public double Time;
    }
    
    private class TouchEntry: ITouchEntity
    {
        public int Id { get; set;  }
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public double Time { get; set; }
        public double InitialTime { get; set; }
        public object CapturedBy { get; private set; }

        public TouchProcessor Processor { get; set; }

        public void Capture(object context)
        {
            CapturedBy = context;
            Processor.ProcessCancelEvent(Id, context);
        }
    }

    private object _lock = new object();
    
    private readonly ITouchClient _touchClient;

    private readonly List<TouchEvent> _touchEvents = new();
    private readonly List<TouchEvent> _touchEventsTemp = new();

    private readonly VelocityTracker _velocityTracker = new();

    private int? _exclusiveTouchId;
    private readonly List<int> _touchesToRemove = new();

    private readonly Dictionary<int, TouchEntry> _touches = new();

    public float Scale { get; internal set; }

    public TouchProcessor(ITouchClient touchClient)
    {
        _touchClient = touchClient;
    }

    public void EnableExclusiveMode(int touchId)
    {
        lock (_lock)
        {
            _exclusiveTouchId = touchId;
        }
    }

    public void DisableExclusiveMode()
    {
        _exclusiveTouchId = null;
    }

    public void AddEvent(TouchEventKind kind, int id, Vector2 position, double time)
    {
        const double differentEventTimeFrame = 1 / 60.0;
        
        lock (_lock)
        {
            if ((_exclusiveTouchId ?? id) != id)
            {
                return;
            }

            if (kind == TouchEventKind.Move && _touchEvents.Count > 0)
            {
                int index = -1;
                for (var idx = _touchEvents.Count - 1; idx >= 0; --idx)
                {
                    if (_touchEvents[idx].Id == id)
                    {
                        index = idx;
                        break;
                    }
                }

                if (index >= 0)
                {
                    var evnt = _touchEvents[index];
                    if (evnt.Kind == TouchEventKind.Move)
                    {
                        if (time - evnt.InitialTime < differentEventTimeFrame)
                        {
                            evnt.Position = position;
                            evnt.Time = time;
                            _touchEvents[index] = evnt;
                            return;
                        }
                    }
                }
            }

            if (kind is TouchEventKind.Down or TouchEventKind.Move or TouchEventKind.Up)
            {
                _velocityTracker.AddTouchMovement(id, position / Scale, time);
            }

            _touchEvents.Add(new TouchEvent
            {
                Id = id,
                Kind = kind,
                Position = position,
                Time = time,
                InitialTime = time
            });
        }
    }
    
    public bool ConsumeEvents()
    {
        var scale = Scale;

        _touchEventsTemp.Clear();

        lock (_lock)
        {
            if(_touchEvents.Count > 0)
            {
                for(var idx = 0; idx < _touchEvents.Count; ++idx)
                {
                    if(_exclusiveTouchId.HasValue && _touchEvents[idx].Id != _exclusiveTouchId.Value)
                    {
                        continue;
                    }

                    _touchEventsTemp.Add(_touchEvents[idx]);
                }

                _touchEvents.Clear();
            }
        }

        if (_exclusiveTouchId.HasValue)
        {
            _touchesToRemove.Clear();
            foreach (var touch in _touches)
            {
                if (touch.Key != _exclusiveTouchId.Value)
                {
                    _touchesToRemove.Add(touch.Key);
                }
            }

            for (var idx = 0; idx < _touchesToRemove.Count; ++idx)
            {
                ProcessCancelEvent(_touchesToRemove[idx]);
            }

            if (_touchesToRemove.Count > 0)
            {
                _velocityTracker.Reset();
            }
            _touchesToRemove.Clear();
        }

        bool change = false;
        for (var idx = 0 ; idx < _touchEventsTemp.Count; ++idx)
        {
            var te = _touchEventsTemp[idx];

            switch (te.Kind)
            {
                case TouchEventKind.Down:
                    ProcessDownEvent(te.Id, te.Position / scale, te.Time);
                    change = true;
                    break;

                case TouchEventKind.Up:
                    ProcessUpEvent(te.Id, te.Position / scale, te.Time);
                    _velocityTracker.Reset();
                    change = true;
                    break;

                case TouchEventKind.Move:
                    ProcessMoveEvent(te.Id, te.Position / scale, te.Time);
                    change = true;
                    break;

                case TouchEventKind.Cancel:
                    ProcessCancelEvent(te.Id);
                    _velocityTracker.Reset();
                    change = true;
                    break;
            }
        }

        if (_touches.Count == 0)
        {
            DisableExclusiveMode();
        }

        return change;
    }

    private void ProcessDownEvent(int id, Vector2 position, double time)
    {
        var entry = new TouchEntry
        {
            Id = id,
            Origin = position,
            Position = position,
            Time = time,
            InitialTime = time,
            Processor = this
        };

        _touches[id] = entry;
        _touchClient.OnDown(entry);
    }

    private void ProcessMoveEvent(int id, Vector2 position, double time)
    {
        if (_touches.TryGetValue(id, out var touch))
        {
            touch.Position = position;
            touch.Time = time;

            var client = touch.CapturedBy as ITouchClient ?? _touchClient;
            client.OnMove(touch);
        }
    }

    private void ProcessUpEvent(int id, Vector2 position, double time)
    {
        if (_touches.TryGetValue(id, out var touch))
        {
            touch.Position = position;
            _touchClient.OnUp(touch);
            _touches.Remove(id);
        }
    }

    public void ProcessCancelEvent(int id, object capturedBy = null)
    {
        _touchClient.OnCancel(id, capturedBy);

        if (_touches.TryGetValue(id, out var touch))
        {
            _touchClient.OnUp(touch);
        }

        if (capturedBy == null)
        {
            _touches.Remove(id);
        }
    }

    public void Reset()
    {
        _touches.Clear();
        _touchClient.Reset();
    }

    public float CalculateHorizontalVelocity(int touchId)
    {
        return -_velocityTracker.CalculateTouchVelocity(touchId, false);
    }
    
    public float CalculateVerticalVelocity(int touchId)
    {
        return -_velocityTracker.CalculateTouchVelocity(touchId, true);
    }

    public void Capture(int touchId, object context)
    {
        _touchClient.OnCancel(touchId, context);
    }
}