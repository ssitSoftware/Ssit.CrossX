using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public abstract class TextBaseHandler<TTextView> : BackgroundHandler<TTextView> where TTextView: Label
{
    private readonly IFontsManager _fontsManager;
    protected readonly TextRenderingContext TextRenderingContext = new ();

    protected virtual RgbaColor? TextColor => AttachedView.TextColor;
    protected virtual RgbaColor? TextOutlineColor => AttachedView.TextOutlineColor;
    
    
    protected RectangleF TextRectangle
    {
        get
        {
            var xx = ScreenBounds.X;
            var yy = ScreenBounds.Y;
            
            var width = ScreenBounds.Width;
            var height = ScreenBounds.Height;

            if (AttachedView.Padding?.Left.HasValue ?? false)
            {
                width -= AttachedView.Padding.Value.Left.Value.Calculate(ScreenBounds.Width);
                xx += AttachedView.Padding.Value.Left.Value.Calculate(ScreenBounds.Width);
            }
            
            if (AttachedView.Padding?.Right.HasValue ?? false)
            {
                width -= AttachedView.Padding.Value.Right.Value.Calculate(ScreenBounds.Width);
            }
            
            if (AttachedView.Padding?.Top.HasValue ?? false)
            {
                height -= AttachedView.Padding.Value.Top.Value.Calculate(ScreenBounds.Height);
                yy += AttachedView.Padding.Value.Top.Value.Calculate(ScreenBounds.Height);
            }
            
            if (AttachedView.Padding?.Bottom.HasValue ?? false)
            {
                height -= AttachedView.Padding.Value.Bottom.Value.Calculate(ScreenBounds.Height);
            }
            
            return new RectangleF(xx, yy, width, height);
        }
    }
    
    public TextBaseHandler(CreateHandlerParameters parameters, IFontsManager fontsManager) : base(parameters)
    {
        _fontsManager = fontsManager;
        if (AttachedView.Text is not null)
        {
            AttachedView.Text.TextChanged += OnTextChanged;
        }
    }

    public override void Init()
    {
        OnTextChanged();
    }

    protected IFont GetFont()
    {
        return _fontsManager.GetFont(AttachedView.Font?.FontFamily ?? "Default", AttachedView.Font?.FontSize ?? 12);
    }
    
    protected virtual void OnTextChanged()
    {
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);

        if (AttachedView.Text is not null)
        {
            AttachedView.Text.TextChanged -= OnTextChanged;
        }
    }

    protected void CalculateSizeInternal(out Length width, out Length height)
    {
        CalculateAlign(out var ha, out var va);
        
        if (AttachedView.Width.HasValue)
        {
            width = AttachedView.Width.Value;
        }
        else if (ha != Align.Fill)
        {
            width = Length.Auto;
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
            height = Length.Auto;
        }
        else
        {
            height = Length.Fill;
        }
    }
    
    public override void CalculateSize(out Length width, out Length height)
    {
        CalculateSizeInternal(out width, out height);
        
        if (width.IsAuto)
        {
            width = TextRenderingContext.Width;
            width = CalculateLengthWithPadding(width, AttachedView.Padding?.Left, AttachedView.Padding?.Right);
        }

        if (height.IsAuto)
        {
            height = TextRenderingContext.Height;
            height = CalculateLengthWithPadding(height, AttachedView.Padding?.Top, AttachedView.Padding?.Bottom);
        }
    }

    public override void CalculateAlign(out Align horizontalAlign, out Align verticalAlign)
    {
        var textAlign = AttachedView.TextAlign ?? ContentAlign.Center | ContentAlign.VCenter;
        
        if (AttachedView.HorizontalAlign is null)
        {
            horizontalAlign = Align.Start;
            
            if ((textAlign & ContentAlign.Center) == ContentAlign.Center)
            {
                horizontalAlign = Align.Center;
            }
            
            if ((textAlign & ContentAlign.Right) == ContentAlign.Right)
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
            
            if ((textAlign & ContentAlign.VCenter) == ContentAlign.VCenter)
            {
                verticalAlign = Align.Center;
            }
            
            if ((textAlign & ContentAlign.Bottom) == ContentAlign.Bottom)
            {
                verticalAlign = Align.End;
            }
        }
        else
        {
            verticalAlign = AttachedView.VerticalAlign.Value;
        }
    }
}