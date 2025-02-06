using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelHandler<TLabel> : TextBaseHandler<TLabel> where TLabel: Label
{
    private readonly IActionDispatcher _actionDispatcher;

    public LabelHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IActionDispatcher actionDispatcher) : base(parameters, fontsManager)
    {
        _actionDispatcher = actionDispatcher;
        OnTextChanged();
    }

    protected sealed override void OnTextChanged()
    {
        var font = GetFont();
        TextRenderingContext.Reset();
        font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, TextRenderingContext);

        CalculateSizeInternal(out var width, out var height);
        CalculateAlign(out var ha, out var va);
        
        if (width.IsAuto && ha != Align.Fill || height.IsAuto && va != Align.Fill)
        {
            Parent?.RecalculateLayout(AttachedView);
        }
    }

    public override void SetBounds(RectangleF rectangleF)
    {
        base.SetBounds(rectangleF);

        var oldWidth = TextRenderingContext.Width * TextScale;
        var oldHeight = TextRenderingContext.Height * TextScale;
        
        var font = GetFont();
        font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, TextRenderingContext);

        var newWidth = TextRenderingContext.Width * TextScale;
        var newHeight = TextRenderingContext.Height * TextScale;

        if (AttachedView.Text.Length > 0 && TextRectangle.Height < 1)
        {
            TextRenderingContext.Reset();
            OnTextChanged();
            
            Parent?.RecalculateLayout(AttachedView);
        }
        
        if (MathF.Abs(oldWidth - newWidth) > float.Epsilon || MathF.Abs(oldHeight - newHeight) > float.Epsilon)
        {
            TextRenderingContext.Reset();
            OnTextChanged();
            
            Parent?.RecalculateLayout(AttachedView);
        }
    }
    
    protected override void OnDraw(IRenderer renderer, RenderMode mode)
    {
        base.OnDraw(renderer, mode);
        
        var font = GetFont();

        try
        {
            renderer.DrawText(
                font: font,
                text: AttachedView.Text,
                position: TextRectangle,
                align: AttachedView.TextAlign ?? ContentAlign.Center | ContentAlign.VCenter,
                scale: TextScale,
                color: TextColor(mode) ?? RgbaColor.Transparent,
                spacing: AttachedView.TextSpacing ?? TextSpacing.Normal,
                outlineColor: TextOutlineColor(mode) ?? RgbaColor.Transparent,
                context: TextRenderingContext);
        }
        catch(Exception)
        {
            TextRenderingContext.Reset();
            _actionDispatcher.Enqueue(OnTextChanged);
        }
    }
}