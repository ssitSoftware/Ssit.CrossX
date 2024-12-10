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
    
    ~ViewHandler()
    {
        OnDispose(false);
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