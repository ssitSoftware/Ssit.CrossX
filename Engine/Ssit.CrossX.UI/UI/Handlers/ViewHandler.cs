using System;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public abstract class ViewHandler: IDisposable
{
    public class CreateHandlerParameters
    {
        public View View;
        public IViewParent Parent;
    }
    
    public View View { get; }
    public Type ViewType { get; }

    public IViewParent Parent { get; }

    public RectangleF Bounds { get; private set; }

    public RectangleF ScreenBounds { get; private set; }
    
    protected ViewHandler(Type type, CreateHandlerParameters parameters)
    {
        View = parameters.View;
        Parent = parameters.Parent;
        ViewType = type;

        if (((IHandlerView)View).Handler != null) throw new InvalidOperationException();
        View.Handler = this;
    }
    
    protected static Length CalculateLengthWithPadding(Length length, Length? pad1, Length? pad2)
    {
        var p1 = pad1 ?? Length.Zero;
        var p2 = pad2 ?? Length.Zero;

        if (p1.IsAuto)
        {
            p1 = Length.Zero;
        }
        
        if (p2.IsAuto)
        {
            p2 = Length.Zero;
        }

        var add = Length.Zero;
        add += p1.Value + p2.Value;

        var percent = p1.Percent + p2.Percent;

        var rest = 1 - percent;
        if (rest <= 0)
        {
            throw new InvalidProgramException("Percent value of paddings cannot 100% or more!");
        }

        var restSize = (length + add).Calculate(0);
        var totalSize = restSize / rest;

        return totalSize;
    }
    
    ~ViewHandler()
    {
        OnDispose(false);
    }

    public virtual void Init()
    {
    }

    public virtual void SetBounds(RectangleF rectangleF)
    {
        Bounds = rectangleF;
        var offset = Parent?.ScreenBounds.TopLeft ?? Vector2.Zero;
        ScreenBounds = new RectangleF(offset + Bounds.TopLeft, Bounds.Size);
    }
    
    public virtual void Draw(IRenderer renderer)
    {
        
    }

    public virtual void Update(float dt)
    {
        
    }
    
    public void Dispose()
    {
        OnDispose(true);
        View.Handler = null;
    }

    protected virtual void OnDispose(bool disposing)
    {
    }

    protected virtual void OnRecalculatePositionAndSize()
    {
        Parent?.RecalculateLayout(View);
    }

    internal void RecalculatePositionAndSize()
    {
        OnRecalculatePositionAndSize();
    }

    public virtual void CalculateSize(out Length width, out Length height)
    {
        width = View.Width ?? Length.Fill;
        height = View.Height ?? Length.Fill;
    }

    public virtual void CalculateAlign(out Align horizontalAlign, out Align verticalAlign)
    {
        horizontalAlign = View.HorizontalAlign ?? Align.Fill;
        verticalAlign = View.VerticalAlign ?? Align.Fill;
    }
}

public abstract class ViewHandler<TView>(ViewHandler.CreateHandlerParameters parameters)
    : ViewHandler(typeof(TView), parameters)
    where TView : View
{
    protected TView AttachedView => (TView)View;
    protected IHandlerView HandlerView => View;
}