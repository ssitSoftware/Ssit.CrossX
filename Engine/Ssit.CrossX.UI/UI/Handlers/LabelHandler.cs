using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelHandler<TLabel> : TextBaseHandler<TLabel> where TLabel: Label
{
    public LabelHandler(ViewHandler.CreateHandlerParameters parameters, IFontsManager fontsManager) : base(parameters, fontsManager)
    {
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