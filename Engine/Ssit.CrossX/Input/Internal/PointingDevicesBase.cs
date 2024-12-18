using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Input.Internal;

public class PointingDevicesBase: IPointingDevices, ITouchClient
{
    private readonly List<Pointer> _pointers = new();

    public readonly TouchProcessor TouchProcessor;
    
    public IReadOnlyList<Pointer> Pointers => _pointers;
    public Pointer GetPointer(int id) => _pointers.Find(o => o.Id == id);
    public Vector2? HoverPosition { get; private set; } = null;

    protected PointingDevicesBase()
    {
        TouchProcessor = new TouchProcessor(this);
    }
    
    protected void SetPointer(int id, ButtonState state, Vector2? position)
    {
        var pointer = GetPointer(id);
        if (pointer == null)
        {
            
            pointer = new Pointer(id);
            pointer.Update(state, position.GetValueOrDefault(), position.GetValueOrDefault());
            _pointers.Add(pointer);
        }
        else
        {
            if (position.HasValue)
            {
                pointer.Update(state, position.Value);
            }
            else
            {
                pointer.Update(state);
            }
        }
    }

    public void OnPreUpdate()
    {
        for (var idx = 0; idx < _pointers.Count;)
        {
            if (_pointers[idx].State == ButtonState.Empty)
            {
                _pointers.RemoveAt(idx);
            }
            ++idx;
        }
        
        for (var idx = 0; idx < _pointers.Count; ++idx)
        {
            if (_pointers[idx].State == ButtonState.JustReleased)
            {
                _pointers[idx].Update(ButtonState.Empty);
            }
        }
        
        for (var idx = 0; idx < _pointers.Count; ++idx)
        {
            if (_pointers[idx].State == ButtonState.JustPressed)
            {
                _pointers[idx].Update(ButtonState.Down);
            }
        }
    }

    public bool OnDown(ITouchEntity entity)
    {
        SetPointer(entity.Id, ButtonState.JustPressed, entity.Position);
        return true;
    }

    public bool OnMove(ITouchEntity entity)
    {
        SetPointer(entity.Id, ButtonState.Down, entity.Position);
        return true;
    }

    public void OnUp(ITouchEntity entity)
    {
        SetPointer(entity.Id, ButtonState.JustReleased, entity.Position);
    }

    public void OnCancel(int id, object capturedBy = null)
    {
        SetPointer(id, ButtonState.JustReleased, null);
    }

    public void Reset()
    {
        foreach (var pointer in _pointers)
        {
            pointer.Update(ButtonState.Empty);
        }
    }
    
    public void UpdateHoverPosition(Vector2? position)
    {
        HoverPosition = position;
    }

    public float CalculateHorizontalVelocity(int id) => TouchProcessor.CalculateHorizontalVelocity(id);

    public float CalculateVerticalVelocity(int id) => TouchProcessor.CalculateVerticalVelocity(id);
}