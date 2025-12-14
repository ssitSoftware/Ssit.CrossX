using System;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class LabelHandler<TLabel> : TextBaseHandler<TLabel> where TLabel: Label
{
    private readonly IActionDispatcher _actionDispatcher;

    public LabelHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IActionDispatcher actionDispatcher, IPaletteSource paletteSource = null) : base(parameters, fontsManager, paletteSource)
    {
        _actionDispatcher = actionDispatcher;
        OnTextChanged();
    }

    protected sealed override void OnTextChanged()
    {
        var font = GetFont();
        TextRenderingContext.Reset();
        font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, AttachedView?.LineSpacing ?? 0, TextRenderingContext);
        
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
        font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, 0, TextRenderingContext);

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
    
    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        
        try
        {
            OnDrawInternal(renderer);
        }
        catch(Exception)
        {
            TextRenderingContext.Reset();
            _actionDispatcher.Enqueue(OnTextChanged);
            Parent?.GetParent<IPage>().InvalidateRendering();
        }
    }

    protected virtual void OnDrawInternal(IRenderer2 renderer)
    {
        DrawText(renderer, TextColor(renderer) ?? RgbaColor.Transparent, TextOutlineColor(renderer) ?? RgbaColor.Transparent, Vector2.Zero);
    }

    protected void DrawText(IRenderer2 renderer, RgbaColor color, RgbaColor outlineColor, Vector2 offset)
    {
        var font = GetFont();
        
        renderer.TextRenderer.DrawText(
            font: font,
            text: AttachedView.Text,
            position: TextRectangle + offset,
            align: AttachedView.TextAlign ?? ContentAlign.Center | ContentAlign.VCenter,
            scale: TextScale,
            color: color,
            spacing: AttachedView.TextSpacing ?? TextSpacing.Normal,
            outlineColor: outlineColor,
            lineSpacing: AttachedView?.LineSpacing ?? 0,
            context: TextRenderingContext);
    }
}