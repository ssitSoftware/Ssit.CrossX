using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class VirtualButtonHandler(ViewHandler.CreateHandlerParameters parameters, IHandlerMapper handlerMapper, IPaletteSource paletteSource, IVirtualGameInput virtualGameInput,
    IHapticDevice hapticDevice, IRenderer2 renderer) 
    : ContainerHandler<VirtualButton>(parameters, handlerMapper, paletteSource), IInputConsumer, IColorSource
{
    public RgbaColor? OutlineColor => null;
    
    private int? _currentPointerId;
    
    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context)
    {
    }

    public bool ProcessInput(IReadOnlyList<Pointer> pointers, IInputContext context)
    {
        if (_currentPointerId.HasValue)
        {
            var pointer = pointers.FirstOrDefault(o => o.Id == _currentPointerId.Value);
            if (pointer is not null)
            {
                virtualGameInput.SetButton(AttachedView.Button, pointer.State);
                
                if (pointer.State == ButtonState.Empty)
                {
                    _currentPointerId = null;
                }
                
                if (pointer.State == ButtonState.JustReleased && true == AttachedView.HapticFeedback?.Value)
                {
                    hapticDevice.Feedback(FeedbackStyle.Release, pointer.OriginalPosition);
                }
                return false;
            }
            
            _currentPointerId = null;
            virtualGameInput.SetButton(AttachedView.Button, ButtonState.Empty);
        }
        
        foreach (var pointer in pointers)
        {
            if (pointer.State == ButtonState.JustPressed)
            {
                if (ScreenBounds.Contains(pointer.Position))
                {
                    _currentPointerId = pointer.Id;
                    context.CapturePointer(pointer.Id, this);
                    
                    virtualGameInput.SetButton(AttachedView.Button, ButtonState.JustPressed);
                    context.CapturePointer(pointer.Id, this);

                    if (true == AttachedView.HapticFeedback?.Value)
                    {
                        hapticDevice.Feedback(FeedbackStyle.Push, pointer.OriginalPosition);
                    }

                    return true;
                }
            }
        }

        return false;
    }

    public void CancelPointer(int pointerId, IInputContext context)
    {
        if (_currentPointerId == pointerId)
        {
            _currentPointerId = null;
            virtualGameInput.SetButton(AttachedView.Button, ButtonState.Empty);
        }
    }

    protected override void OnDispose(bool disposing)
    {
        virtualGameInput.SetButton(AttachedView.Button, ButtonState.Empty);
        base.OnDispose(disposing);
    }

    public RgbaColor? GetColor(string id)
    {
        switch (id)
        {
            case nameof(VirtualButton.ColorPressed):
                return _currentPointerId.HasValue ? AttachedView.ColorPressed.GetColor(PaletteSource, renderer) : null;
            
            case nameof(VirtualButton.OutlineColorPressed):
                return _currentPointerId.HasValue ? AttachedView.OutlineColorPressed.GetColor(PaletteSource, renderer) : null;
        }

        return null;
    }
}