using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class ContainerHandler(ViewHandler.CreateHandlerParameters parameters, IHandlerMapper handlerMapper)
    : ChildrenContainerHandler<Container>(parameters, handlerMapper)
{
    protected override void RecalculateChildrenLayouts()
    {
        if (RecalculateChildren.Count == 0)
            return;

        foreach (var child in RecalculateChildren)
        {
            CalculateChildPosition(child);
        }
        RecalculateChildren.Clear();
    }
    
    private void CalculateChildPosition(View child)
    {
        var handlerView = (IHandlerView)child;
        
        var x = child.AnchorX ?? Length.Auto;
        var y = child.AnchorY ?? Length.Auto;

        handlerView.Handler.CalculateSize(out var width, out var height);
        handlerView.Handler.CalculateAlign(out var horizontalAlign, out var verticalAlign);

        var bounds = CalculateTargetBounds(Bounds);

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
        
        var xx = bounds.X + x.Calculate(bounds.Width);
        var yy = bounds.Y + y.Calculate(bounds.Height);
        var ww = width.Calculate(bounds.Width);
        var hh = height.Calculate(bounds.Height);
        
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
        
        handlerView.Handler.SetBounds(new RectangleF(xx, yy, ww, hh));
    }
}