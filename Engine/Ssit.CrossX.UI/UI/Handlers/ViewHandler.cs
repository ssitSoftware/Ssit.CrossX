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
    protected IRenderModeProvider RenderModeProvider { get; }

    public IViewParent Parent { get; }

    protected RectangleF Bounds { get; private set; }

    public RectangleF ScreenBounds { get; private set; }

    protected float CurrentScale => Parent.GetParent<IPage>().Scale;
    
    protected ViewHandler(Type type, CreateHandlerParameters parameters, IRenderModeProvider renderModeProvider)
    {
        View = parameters.View;
        Parent = parameters.Parent;
        ViewType = type;
        RenderModeProvider = renderModeProvider;

        if (((IHandlerView)View).Handler != null) throw new InvalidOperationException();
        View.Handler = this;
    }
    
    protected Length CalculateLengthWithPadding(Length length, Length? pad1, Length? pad2)
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

        var restSize = (length + add).Calculate(CurrentScale, 0);
        var totalSize = MathF.Ceiling(restSize / rest);

        return new Length(pixels: totalSize);
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
        if (RenderModeProvider.RenderMode == RenderMode.Debug)
        {
            OnDrawDebug(renderer);
        }
        else
        {
            OnDraw(renderer);
        }
    }

    protected virtual void OnDrawDebug(IRenderer renderer)
    {
        renderer.FillRectangle(ScreenBounds, RgbaColor.Magenta * 0.2f);
    }

    public virtual void Update(float dt)
    {
        
    }

    protected virtual void OnDraw(IRenderer renderer)
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

    protected void SignalRecalculationPending()
    {
        var page = Parent?.GetParent<IPage>();
        if (page is not null)
        {
            page.SignalRecalculationPending();
        }
    }
}

public abstract class ViewHandler<TView>(ViewHandler.CreateHandlerParameters parameters, IRenderModeProvider renderModeProvider)
    : ViewHandler(typeof(TView), parameters, renderModeProvider)
    where TView : View
{
    protected TView AttachedView => (TView)View;
    protected IHandlerView HandlerView => View;
}