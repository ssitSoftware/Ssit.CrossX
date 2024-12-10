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

        foreach (var child in AttachedView.Children)
        {
            CalculateChildPosition(child);
        }
    }
    
    private void CalculateChildPosition(View child)
    {
        var x = child.AnchorX ?? Length.Zero;
        var y = child.AnchorY ?? Length.Zero;
        
        var width = child.Width ?? Length.Auto;
        var height = child.Height ?? Length.Auto;
        
        var verticalAlign = child.VerticalAlign ?? Align.Fill;
        var horizontalAlign = child.HorizontalAlign ?? Align.Fill;

        var bounds = CalculateTargetBounds(Bounds);
        
        var xx = x.Calculate(bounds.Width);
        var yy = y.Calculate(bounds.Height);
        var ww = width.Calculate(bounds.Width);
        var hh = height.Calculate(bounds.Height);
        
        switch (horizontalAlign)
        {
            case Align.Fill:
                ww = bounds.Width;
                xx = bounds.X;
                break;
            
            case Align.Start:
                xx = bounds.X;
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
                yy = bounds.Y;
                break;
            
            case Align.Start:
                yy = bounds.Y;
                break;
            
            case Align.Center:
                yy -= hh / 2f;
                break;
            
            case Align.End:
                yy -= hh;
                break;
        }

        var handlerView = (IHandlerView)child;
        handlerView.Handler.SetBounds(new RectangleF(xx, yy, ww, hh));
    }
}