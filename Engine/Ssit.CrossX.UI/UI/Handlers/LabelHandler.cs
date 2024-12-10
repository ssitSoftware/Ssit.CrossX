using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelHandler : ViewHandler<Label>
{
    private readonly TextRenderingContext _textRenderingContext = new ();
    
    public LabelHandler(CreateHandlerParameters parameters) : base(parameters)
    {
        if (AttachedView.Text is not null)
        {
            AttachedView.Text.TextChanged += OnTextChanged;
            
            var font = GetFont();
            font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, _textRenderingContext);
        }
    }

    private IFont GetFont()
    {
        return null;
    }
    
    private void OnTextChanged()
    {
        var font = GetFont();
        font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, _textRenderingContext);

        CalculateAlign(out var ha, out var va);
        
        if ((AttachedView.Width?.IsAuto ?? true) && ha != Align.Fill || (AttachedView.Height?.IsAuto ?? true) && va != Align.Fill)
        {
            Parent?.RecalculateLayout(AttachedView);
        }
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);

        if (AttachedView.Text is not null)
        {
            AttachedView.Text.TextChanged -= OnTextChanged;
        }
    }

    public override void CalculateSize(out Length width, out Length height)
    {
        CalculateAlign(out var ha, out var va);
        
        if (AttachedView.Width.HasValue)
        {
            width = AttachedView.Width.Value;
        }
        else if (ha != Align.Fill)
        {
            width = _textRenderingContext.Width;
        }
        else
        {
            width = Length.Fill;
        }

        if (AttachedView.Height.HasValue)
        {
            height = AttachedView.Height.Value;
        }
        else if(va != Align.Fill)
        {
            height = _textRenderingContext.Height;
        }
        else
        {
            height = Length.Fill;
        }
    }

    public override void CalculateAlign(out Align horizontalAlign, out Align verticalAlign)
    {
        var textAlign = AttachedView.TextAlign ?? TextAlign.Center | TextAlign.VCenter;
        
        if (AttachedView.HorizontalAlign is null)
        {
            horizontalAlign = Align.Start;
            
            if ((textAlign & TextAlign.Center) == TextAlign.Center)
            {
                horizontalAlign = Align.Center;
            }
            
            if ((textAlign & TextAlign.Right) == TextAlign.Right)
            {
                horizontalAlign = Align.End;
            }
        }
        else
        {
            horizontalAlign = AttachedView.HorizontalAlign.Value;
        }
        
        if (AttachedView.VerticalAlign is null)
        {
            verticalAlign = Align.Start;
            
            if ((textAlign & TextAlign.VCenter) == TextAlign.VCenter)
            {
                verticalAlign = Align.Center;
            }
            
            if ((textAlign & TextAlign.Bottom) == TextAlign.Bottom)
            {
                verticalAlign = Align.End;
            }
        }
        else
        {
            verticalAlign = AttachedView.VerticalAlign.Value;
        }
    }

    public override void Draw(IRenderer renderer)
    {
        var font = GetFont();
        renderer.DrawText(
            font: font, 
            text:AttachedView.Text, 
            position: ScreenBounds, 
            align: AttachedView.TextAlign ?? TextAlign.Center | TextAlign.VCenter, 
            color: AttachedView.TextColor, 
            spacing: AttachedView.TextSpacing ?? TextSpacing.Normal, 
            outlineColor: AttachedView.TextOutlineColor, 
            context: _textRenderingContext);
    }
}