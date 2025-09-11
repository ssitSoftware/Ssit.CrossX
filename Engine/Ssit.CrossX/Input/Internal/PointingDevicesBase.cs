using System.Collections.Generic;
using System.Numerics;

namespace Ssit.CrossX.Input.Internal;

public class PointingDevicesBase: IPointingDevices, ITouchClient
{
    public bool Enable { get; set; } = true;
    private readonly List<Pointer> _pointers = new();

    public readonly TouchProcessor TouchProcessor;
    
    public IReadOnlyList<Pointer> Pointers => _pointers;
    public Pointer GetPointer(int id) => _pointers.Find(o => o.Id == id);
    public Vector2? HoverPosition { get; private set; }
    
    public bool LockMouseInWindow { get; set; }

    public bool ShowHoverPointer { get; set; } = true;

    private readonly Stack<Vector2?> _hoverPositions = new();
    
    protected PointingDevicesBase()
    {
        TouchProcessor = new TouchProcessor(this);
    }
    
    private void SetPointer(int id, ButtonState state, Vector2? position)
    {
        if (!Enable)
            return;
        
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
        if (!Enable)
            return false;
        
        SetPointer(entity.Id, ButtonState.JustPressed, entity.Position);
        return true;
    }

    public bool OnMove(ITouchEntity entity)
    {
        if (!Enable)
            return false;
        
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
        if (!Enable)
        {
            ShowPointer(!position.HasValue);
            return;
        }

        HoverPosition = position;
        
        if (ShowHoverPointer)
        {
            ShowPointer(true);
        }
        else
        {
            ShowPointer(!position.HasValue);
        }
    }

    protected virtual void ShowPointer(bool show)
    {
        
    }

    public virtual void SetHoverPosition(Vector2 position)
    {
    }

    public void PushTransform(Matrix3x2 transform)
    {
        _hoverPositions.Push(HoverPosition);

        if (HoverPosition.HasValue)
        {
            HoverPosition = Vector2.Transform(HoverPosition.Value, transform);
        }
        
        foreach (var ptr in Pointers)
        {
            ptr.PushTransform(transform);
        }
    }

    public void PopTransform()
    {
        foreach (var ptr in Pointers)
        {
            ptr.PopTransform();
        }
        HoverPosition = _hoverPositions.Pop();
    }

    public float CalculateHorizontalVelocity(int id) => TouchProcessor.CalculateHorizontalVelocity(id);

    public float CalculateVerticalVelocity(int id) => TouchProcessor.CalculateVerticalVelocity(id);
}