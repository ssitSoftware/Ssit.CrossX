using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.Input.Internal;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class ScrollViewHandler<TScrollView> : BackgroundHandler<TScrollView>, IViewParent, IInputConsumer, IChildrenContainer where TScrollView: ScrollView
{
    private const float InertiaDecay = 8f;
    private const float InertiaStopThreshold = 1f; // pixels/sec squared
    private const float BounceSpringRate = 36f;
    
    private ViewHandler _contentHandler;

    private readonly VelocityTracker _velocityTracker = new();

    private Vector2 _scrollOffset = Vector2.Zero;
    private Vector2 _velocity = Vector2.Zero;
    private int? _trackingPointerId;
    private Vector2 _lastPointerPosition;
    private SizeF _contentSize;
    private float _autoScrollPausedTimer;

    private List<ViewHandler> _children;
    IReadOnlyList<ViewHandler> IChildrenContainer.Children => _children;

    private float ScrollExceed =>
        AttachedView?.ScrollExceed?.Calculate(CurrentScale, Math.Max(Bounds.Width, Bounds.Height)) ?? 0f;

    public ScrollViewHandler(CreateHandlerParameters parameters, IHandlerMapper handlerMapper, IPaletteSource paletteSource = null) : base(parameters, paletteSource)
    {
        _contentHandler = handlerMapper.Create(AttachedView.ContentView, this);
        _children = [_contentHandler];
        _autoScrollPausedTimer = AttachedView.AutoScrollResumeDelay;
    }

    public override void Update(float dt)
    {
        if (!_trackingPointerId.HasValue)
        {
            if (_autoScrollPausedTimer > 0)
                _autoScrollPausedTimer -= dt;

            if (_velocity != Vector2.Zero)
            {
                _scrollOffset += _velocity * dt;

                var maxOffset = GetMaxScrollOffset();

                if (_scrollOffset.X < 0 || _scrollOffset.X > maxOffset.maxX && _velocity.X != 0)
                {
                    _velocity = _velocity with { X = 0 };
                }
                
                if (_scrollOffset.Y < 0 || _scrollOffset.Y > maxOffset.maxY && _velocity.Y != 0)
                {
                    _velocity = _velocity with { Y = 0 };
                }
                
                if (ScrollExceed > 0)
                    RubberBandScrollOffset(ScrollExceed);
                else
                    ClampScrollOffset();
                
                _velocity *= MathF.Exp(-InertiaDecay * dt);
                if (_velocity.LengthSquared() < InertiaStopThreshold)
                    _velocity = Vector2.Zero;
            }
            else
            {
                var (maxX, maxY) = GetMaxScrollOffset();
                var clampedOffset = new Vector2(Math.Clamp(_scrollOffset.X, 0f, maxX),
                    Math.Clamp(_scrollOffset.Y, 0f, maxY));

                if (_scrollOffset != clampedOffset)
                {
                    var springFactor = 1f - MathF.Exp(-BounceSpringRate * dt);
                    _scrollOffset = Vector2.Lerp(_scrollOffset, clampedOffset, springFactor);
                    _velocity = Vector2.Zero;

                    if ((_scrollOffset - clampedOffset).LengthSquared() < 0.25f)
                        _scrollOffset = clampedOffset;
                }
                else if (_autoScrollPausedTimer <= 0 && (AttachedView.AutoScrollSpeedX.HasValue || AttachedView.AutoScrollSpeedY.HasValue))
                {
                    var reference = Math.Max(Bounds.Width, Bounds.Height);
                    if (AttachedView.AutoScrollSpeedX.HasValue)
                        _scrollOffset.X += AttachedView.AutoScrollSpeedX.Value.Calculate(CurrentScale, reference) * dt;
                    if (AttachedView.AutoScrollSpeedY.HasValue)
                        _scrollOffset.Y += AttachedView.AutoScrollSpeedY.Value.Calculate(CurrentScale, reference) * dt;
                    ClampScrollOffset();
                }
            }
        }
        else
        {
            _autoScrollPausedTimer = AttachedView.AutoScrollResumeDelay;
        }

        var child = AttachedView.ContentView;
        if (child is not null)
        {
            var handlerView = (IHandlerView)child;
            handlerView.Handler.Update(dt);
        }

        RecalculateChildrenLayouts();
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);

        renderer.StateManager.SaveState();
        renderer.StateManager.SetClipRect(ScreenBounds);

        
        _contentHandler?.Draw(renderer);

        renderer.StateManager.RestoreState();
    }

    protected virtual void RecalculateChildrenLayouts()
    {
        if (AttachedView.ContentView == null)
            return;

        CalculateChildPosition(AttachedView.ContentView);
    }

    private void CalculateChildPosition(View child)
    {
        var handlerView = (IHandlerView)child;

        var x = child.AnchorX ?? Length.Auto;
        var y = child.AnchorY ?? Length.Auto;

        handlerView.Handler.CalculateSize(out var width, out var height);
        handlerView.Handler.CalculateAlign(out var horizontalAlign, out var verticalAlign);

        var bounds = CalculateTargetBounds();

        if (x.IsAuto)
        {
            switch (horizontalAlign)
            {
                case Align.End:
                    x = Length.Fill;
                    break;

                case Align.Center:
                    x = new Length(0, 0.5f);
                    break;

                case Align.Fill:
                    x = Length.Zero;
                    break;
            }
        }

        if (y.IsAuto)
        {
            switch (verticalAlign)
            {
                case Align.End:
                    y = Length.Fill;
                    break;

                case Align.Center:
                    y = new Length(0, 0.5f);
                    break;

                case Align.Fill:
                    y = Length.Zero;
                    break;
            }
        }

        var xx = bounds.X + x.Calculate(CurrentScale, bounds.Width);
        var yy = bounds.Y + y.Calculate(CurrentScale, bounds.Height);
        var ww = width.Calculate(CurrentScale, bounds.Width);
        var hh = height.Calculate(CurrentScale, bounds.Height);

        switch (horizontalAlign)
        {
            case Align.Fill:
                ww = bounds.Width;
                break;

            case Align.Start:
                break;

            case Align.Center:
                xx -= ww / 2f;
                break;

            case Align.End:
                xx -= ww;
                break;
        }

        switch (verticalAlign)
        {
            case Align.Fill:
                hh = bounds.Height;
                break;

            case Align.Start:
                break;

            case Align.Center:
                yy -= hh / 2f;
                break;

            case Align.End:
                yy -= hh;
                break;
        }

        _contentSize = new SizeF(ww, hh);
        handlerView.Handler.SetBounds(new RectangleF(xx - _scrollOffset.X, yy - _scrollOffset.Y, ww, hh));
    }

    public void RecalculateLayout(View view = null)
    {
        if (AttachedView.ContentView is IViewParent parent)
        {
            parent.RecalculateLayout();
        }
        SignalRecalculationPending();
    }

    public RectangleF CalculateTargetBounds()
    {
        return new RectangleF(0, 0, Bounds.Width, Bounds.Height);
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _contentHandler?.Dispose();
        _contentHandler = null;
        _children = [];
    }

    public override void SetBounds(RectangleF rectangleF)
    {
        base.SetBounds(rectangleF);
        RecalculateLayout();
    }

    TParent IViewParent.GetParent<TParent>(bool optional)
    {
        if (this is TParent parent)
        {
            return parent;
        }
        return Parent.GetParent<TParent>(optional);
    }

    private (float maxX, float maxY) GetMaxScrollOffset() =>
        (Math.Max(0f, _contentSize.Width - Bounds.Width),
         Math.Max(0f, _contentSize.Height - Bounds.Height));

    private void ClampScrollOffset()
    {
        var (maxX, maxY) = GetMaxScrollOffset();
        _scrollOffset = new Vector2(
            Math.Clamp(_scrollOffset.X, 0f, maxX),
            Math.Clamp(_scrollOffset.Y, 0f, maxY)
        );
    }

    private void ClampScrollOffset(float exceed)
    {
        var (maxX, maxY) = GetMaxScrollOffset();
        _scrollOffset = new Vector2(
            Math.Clamp(_scrollOffset.X, -exceed, maxX + exceed),
            Math.Clamp(_scrollOffset.Y, -exceed, maxY + exceed)
        );
    }

    private void RubberBandScrollOffset(float exceed)
    {
        var (maxX, maxY) = GetMaxScrollOffset();
        _scrollOffset = new Vector2(
            RubberBand(_scrollOffset.X, 0f, maxX, exceed),
            RubberBand(_scrollOffset.Y, 0f, maxY, exceed)
        );
    }

    // Maps excess drag beyond [min,max] to a resistance curve that asymptotically approaches ±exceed.
    private static float RubberBand(float value, float min, float max, float exceed)
    {
        if (value < min)
        {
            var over = min - value;
            return min - exceed * over / (over + exceed);
        }
        if (value > max)
        {
            var over = value - max;
            return max + exceed * over / (over + exceed);
        }
        return value;
    }

    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context)
    {
    }

    public bool ProcessInput(IReadOnlyList<Pointer> pointers, IInputContext context)
    {
        var scrollMode = AttachedView.ScrollMode;
        if (scrollMode == ScrollMode.None) return false;

        if (_trackingPointerId.HasValue)
        {
            var pointer = pointers.FirstOrDefault(o => o.Id == _trackingPointerId.Value);
            if (pointer != null)
            {
                var delta = _lastPointerPosition - pointer.Position;
                if ((scrollMode & ScrollMode.Horizontal) == 0) delta = new Vector2(0, delta.Y);
                if ((scrollMode & ScrollMode.Vertical) == 0) delta = new Vector2(delta.X, 0);

                _scrollOffset += delta;
                ClampScrollOffset(ScrollExceed);

                if (delta != Vector2.Zero)
                {
                    CalculateChildPosition(AttachedView.ContentView);
                }

                _velocityTracker.AddTouchMovement(_trackingPointerId.Value, pointer.Position, DateTime.Now.TimeOfDay.TotalSeconds);
                _lastPointerPosition = pointer.Position;

                if (pointer.State == ButtonState.JustReleased)
                {
                    var vx = (scrollMode & ScrollMode.Horizontal) != 0
                        ? -_velocityTracker.CalculateTouchVelocity(_trackingPointerId.Value, false)
                        : 0f;
                    var vy = (scrollMode & ScrollMode.Vertical) != 0
                        ? -_velocityTracker.CalculateTouchVelocity(_trackingPointerId.Value, true)
                        : 0f;
                    _velocity = new Vector2(vx, vy);
                    _velocityTracker.Reset();
                    _trackingPointerId = null;
                }
                else if (pointer.State == ButtonState.Empty)
                {
                    _velocityTracker.Reset();
                    _trackingPointerId = null;
                }
                return false;
            }
            _velocityTracker.Reset();
            _trackingPointerId = null;
        }

        foreach (var pointer in pointers)
        {
            if (pointer.State == ButtonState.JustPressed && ScreenBounds.Contains(pointer.Position))
            {
                _trackingPointerId = pointer.Id;
                _lastPointerPosition = pointer.Position;
                _velocity = Vector2.Zero;
                _velocityTracker.Reset();
                context.CapturePointer(pointer.Id, this);
                return true;
            }
        }

        return false;
    }

    public void CancelPointer(int pointerId, IInputContext context)
    {
        if (_trackingPointerId == pointerId)
        {
            _trackingPointerId = null;
            _velocity = Vector2.Zero;
            _velocityTracker.Reset();
        }
    }
}
