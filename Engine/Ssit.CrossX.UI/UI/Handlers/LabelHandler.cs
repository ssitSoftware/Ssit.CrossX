using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelHandler<TLabel> : TextBaseHandler<TLabel> where TLabel: Label
{
    public LabelHandler(CreateHandlerParameters parameters, IFontsManager fontsManager) : base(parameters, fontsManager)
    {
        OnTextChanged();
    }

    protected sealed override void OnTextChanged()
    {
        var font = GetFont();
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

        var oldWidth = TextRenderingContext.Width;
        var oldHeight = TextRenderingContext.Height;
        
        var font = GetFont();
        font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, TextRenderingContext);

        var newWidth = TextRenderingContext.Width;
        var newHeight = TextRenderingContext.Height;

        if (AttachedView.Text.Length > 0 && TextRectangle.Height < 1)
        {
            Parent?.RecalculateLayout(AttachedView);
        }
        
        if (MathF.Abs(oldWidth - newWidth) > float.Epsilon || MathF.Abs(oldHeight - newHeight) > float.Epsilon)
        {
            Parent?.RecalculateLayout(AttachedView);
        }
    }
    
    public override void Draw(IRenderer renderer)
    {
        base.Draw(renderer);
        
        var font = GetFont();
        renderer.DrawText(
            font: font,
            text:AttachedView.Text,
            position: TextRectangle,
            align: AttachedView.TextAlign ?? ContentAlign.Center | ContentAlign.VCenter,
            color: TextColor ?? RgbaColor.Transparent,
            spacing: AttachedView.TextSpacing ?? TextSpacing.Normal,
            outlineColor: TextOutlineColor ?? RgbaColor.Transparent,
            context: TextRenderingContext);
    }
}