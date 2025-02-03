using System;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class TextViewHandler : TextBaseHandler<TextView>
{
    private readonly IIoCContainer _container;
    private IVertexBuffer[] _vertexBuffers;
    private bool _recalculateVertices;
    
    public TextViewHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IIoCContainer container) : base(parameters, fontsManager)
    {
        _container = container;
        OnTextChanged();
    }

    private void UpdateText()
    {
        var font = (IGlyphFont)GetFont();
        font.CalculateText(AttachedView.Text, AttachedView.TextSpacing ?? TextSpacing.Normal, TextRenderingContext);

        var paragraphSpacing = AttachedView.ParagraphSpacing?.Calculate(CurrentScale, font.LineSize) ?? 0;
        
        CalculateSizeInternal(out var width, out var height);
        CalculateAlign(out var ha, out var va);

        if (width.IsAuto && ha != Align.Fill)
        {
            font.CalculateMultilineText(AttachedView.Text, AttachedView?.TextSpacing ?? TextSpacing.Normal, 
                float.MaxValue, paragraphSpacing, TextRenderingContext);
        }
        else
        {
            var maxWidth = TextRectangle.Width;

            if (maxWidth < font.Size)
            {
                TextRenderingContext.Update("", font, TextSpacing.Normal, 0);
                return;
            }
            
            font.CalculateMultilineText(AttachedView.Text, AttachedView?.TextSpacing ?? TextSpacing.Normal, 
                maxWidth, paragraphSpacing, TextRenderingContext);
        }

        _recalculateVertices = true;
    }
    
    protected sealed override void OnTextChanged()
    {
        UpdateText();
        
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

    public override void SetBounds(RectangleF rectangleF)
    {
        base.SetBounds(rectangleF);

        var oldWidth = TextRenderingContext.Width * TextScale;
        var oldHeight = TextRenderingContext.Height * TextScale;
        
        UpdateText();

        var newWidth = TextRenderingContext.Width * TextScale;
        var newHeight = TextRenderingContext.Height * TextScale;

        if (AttachedView.Text.Length > 0 && TextRectangle.Height < 1)
        {
            Parent?.RecalculateLayout(AttachedView);
        }
        
        if (MathF.Abs(oldWidth - newWidth) > float.Epsilon || MathF.Abs(oldHeight - newHeight) > float.Epsilon)
        {
            Parent?.RecalculateLayout(AttachedView);
        }
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        if (AttachedView.Text.Length > 0 && MathF.Ceiling(TextRectangle.Height) < MathF.Max(1, TextRenderingContext.Height * TextScale))
        {
           Parent?.RecalculateLayout(AttachedView);
        }
    }

    public override void Draw(IRenderer renderer)
    {
        base.Draw(renderer);
        
        if ( _recalculateVertices || _vertexBuffers == null)
        {
            RecalculateVertices();
        }

        if (_vertexBuffers is null)
            return;

        var font = (IGlyphFont)GetFont();

        var textColor = TextColor ?? RgbaColor.Transparent;
        var outlineColor = TextOutlineColor ?? RgbaColor.Transparent;

        if (textColor.A > 0)
        {
            foreach (var vb in _vertexBuffers)
            {
                renderer.DrawPrimitives(vb, 0, vb.Length, font.FontSheet, textColor);
            }
        }

        if (outlineColor.A > 0 && font.OutlineSheet is not null)
        {
            foreach (var vb in _vertexBuffers)
            {
                renderer.DrawPrimitives(vb, 0, vb.Length, font.OutlineSheet, outlineColor);
            }
        }
    }

    private void RecalculateVertices()
    {
        _recalculateVertices = false;
        
        var font = (IGlyphFont)GetFont();
        var paragraphSpacing = AttachedView.ParagraphSpacing?.Calculate(CurrentScale, font.LineSize) ?? 0;
        
        _vertexBuffers = font.CreateMultilineTextPrimitives(_container, AttachedView.Text, TextRectangle, 
            AttachedView.TextAlign ?? ContentAlign.Left,
            TextScale,
            AttachedView.TextSpacing ?? TextSpacing.Normal,
            paragraphSpacing, 0, TextRenderingContext, _vertexBuffers);
    }
}