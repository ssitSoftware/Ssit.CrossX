using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Text;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;
using Ssit.CrossX.UI.Views.Markdown;

namespace Ssit.CrossX.UI.Handlers.Markdown;

public class MarkdownViewHandler<TMarkdownView> : BackgroundHandler<TMarkdownView> where TMarkdownView: MarkdownView
{
    private readonly IFontsManager _fontsManager;
    private readonly IContentManager _contentManager;
    private readonly IColorSource _colorSource;

    private List<MarkdownBlock> _blocks = new();
    private readonly List<LayoutLine> _layoutLines = new();
    private float _totalHeight;
    private float _lastLayoutWidth = -1;
    private readonly Dictionary<string, ResourceHandle<ITexture>> _textures = new();

    public MarkdownViewHandler(
        CreateHandlerParameters parameters,
        IFontsManager fontsManager,
        IContentManager contentManager,
        IPaletteSource paletteSource = null)
        : base(parameters, paletteSource)
    {
        _fontsManager = fontsManager;
        _contentManager = contentManager;
        _colorSource = parameters.Parent?.GetParent<IColorSource>(true);

        if (AttachedView.Text != null)
        {
            AttachedView.Text.TextChanged += OnTextChanged;
        }
    }

    public override void Init()
    {
        OnTextChanged();
    }

    private void OnTextChanged()
    {
        _blocks = ParseMarkdown(AttachedView.Text?.ToString() ?? "");
        _lastLayoutWidth = -1;
        _layoutLines.Clear();
        _totalHeight = 0;
        Parent?.RecalculateLayout(AttachedView);
    }

    public override void CalculateSize(out Length width, out Length height)
    {
        if (AttachedView.Width.HasValue)
            width = AttachedView.Width.Value;
        else
            width = Length.Fill;

        if (AttachedView.Height is { IsAuto: false })
        {
            height = AttachedView.Height.Value;
            return;
        }

        // If we have a fixed pixel width, pre-calculate height now.
        if (!width.IsAuto && width.Percent == 0 && width.Value > 0)
        {
            var pixelWidth = width.Calculate(CurrentScale, 0);
            var (padL, padR) = GetHorizontalPadding(pixelWidth);
            var contentWidth = MathF.Max(pixelWidth - padL - padR, 0f);
            if (contentWidth > 0 && MathF.Abs(contentWidth - _lastLayoutWidth) > 0.5f)
            {
                LayoutContent(contentWidth);
            }
        }

        if (_totalHeight > 0)
        {
            var (padT, padB) = GetVerticalPadding();
            height = new Length(pixels: _totalHeight + padT + padB);
        }
        else
        {
            height = Length.Auto;
        }
    }

    public override void SetBounds(RectangleF rectangleF)
    {
        base.SetBounds(rectangleF);

        var contentRect = ContentRect;
        var w = contentRect.Width;
        if (w > 0 && MathF.Abs(w - _lastLayoutWidth) > 0.5f)
        {
            LayoutContent(w);
            Parent?.RecalculateLayout(AttachedView);
        }
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);

        var textColor = AttachedView.TextColor?.GetColor(PaletteSource, renderer, _colorSource) ?? RgbaColor.White;
        var outlineColor = AttachedView.TextOutlineColor?.GetColor(PaletteSource, renderer, _colorSource);

        var align = AttachedView.TextAlign;
        var cr = ContentRect;
        var contentWidth = cr.Width;

        var originX = cr.X;
        var originY = cr.Y;

        // Vertical offset — only when height is explicitly set
        if (AttachedView.Height.HasValue && _totalHeight < cr.Height)
        {
            if ((align & ContentAlign.VCenter) == ContentAlign.VCenter)
                originY += (cr.Height - _totalHeight) / 2f;
            else if ((align & ContentAlign.Bottom) == ContentAlign.Bottom)
                originY += cr.Height - _totalHeight;
        }

        var isJustified = (align & ContentAlign.Justified) == ContentAlign.Justified;
        var isRight     = (align & ContentAlign.Right)     == ContentAlign.Right;
        var isCenter    = (align & ContentAlign.Center)    == ContentAlign.Center;

        foreach (var line in _layoutLines)
        {
            // Horizontal offset for this line
            float lineOffsetX;
            if (isJustified && !line.IsLastInBlock && line.Pieces.Count > 1)
                lineOffsetX = 0; // justified: handled per-piece below
            else if (isRight)
                lineOffsetX = contentWidth - line.Width;
            else if (isCenter)
                lineOffsetX = (contentWidth - line.Width) / 2f;
            else
                lineOffsetX = 0;

            var justifiedGap = isJustified && !line.IsLastInBlock && line.Pieces.Count > 1
                ? (contentWidth - line.Width) / (line.Pieces.Count - 1)
                : 0f;

            for (var pi = 0; pi < line.Pieces.Count; pi++)
            {
                var piece = line.Pieces[pi];
                var pieceX = piece.X + lineOffsetX + pi * justifiedGap;

                if (piece.Texture != null)
                {
                    var targetRect = new RectangleF(
                        originX + pieceX,
                        originY + line.Y + (line.Height - piece.TextureSize.Height) / 2f,
                        piece.TextureSize.Width,
                        piece.TextureSize.Height);
                    renderer.SpriteRenderer.Draw(piece.Texture, targetRect);
                }
                else if (piece.Text != null && piece.Font != null)
                {
                    var pos = new Vector2(originX + pieceX, originY + line.Y + line.Height / 2f);
                    renderer.TextRenderer.DrawText(
                        font: piece.Font,
                        text: (TextSource)piece.Text,
                        position: pos,
                        align: ContentAlign.Left | ContentAlign.VCenter,
                        scale: piece.FontScale,
                        color: textColor,
                        outlineColor: outlineColor);
                }
            }
        }
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);

        if (AttachedView.Text != null)
        {
            AttachedView.Text.TextChanged -= OnTextChanged;
        }

        foreach (var handle in _textures.Values)
        {
            handle.Dispose();
        }
        _textures.Clear();
    }

    // ── Padding helpers ───────────────────────────────────────────────────────

    private RectangleF ContentRect
    {
        get
        {
            var x = ScreenBounds.X;
            var y = ScreenBounds.Y;
            var w = ScreenBounds.Width;
            var h = ScreenBounds.Height;

            var padding = AttachedView.Padding;
            if (padding.HasValue)
            {
                var p = padding.Value;
                var pl = p.Left?.Calculate(CurrentScale, w) ?? 0f;
                var pr = p.Right?.Calculate(CurrentScale, w) ?? 0f;
                var pt = p.Top?.Calculate(CurrentScale, h) ?? 0f;
                var pb = p.Bottom?.Calculate(CurrentScale, h) ?? 0f;
                x += pl; y += pt; w -= pl + pr; h -= pt + pb;
            }

            return new RectangleF(x, y, MathF.Max(w, 0f), MathF.Max(h, 0f));
        }
    }

    private (float top, float bottom) GetVerticalPadding()
    {
        var padding = AttachedView.Padding;
        if (!padding.HasValue) return (0f, 0f);
        var p = padding.Value;
        return (p.Top?.Calculate(CurrentScale, 0f) ?? 0f,
                p.Bottom?.Calculate(CurrentScale, 0f) ?? 0f);
    }

    private (float left, float right) GetHorizontalPadding(float refWidth)
    {
        var padding = AttachedView.Padding;
        if (!padding.HasValue) return (0f, 0f);
        var p = padding.Value;
        return (p.Left?.Calculate(CurrentScale, refWidth) ?? 0f,
                p.Right?.Calculate(CurrentScale, refWidth) ?? 0f);
    }

    // ── Font helpers ──────────────────────────────────────────────────────────

    private (IFont font, float scale) GetStyledFont(MarkdownStyle style)
    {
        var mapper = AttachedView.Mapper;
        FontDesc desc = mapper != null
            ? mapper.GetFont(style)
            : new FontDesc { FontFamily = "Default", FontSize = 12 };

        var requestedSize = desc.FontSize > 0 ? (float)desc.FontSize : 12f;

        if (AttachedView.Scaling == TextScaling.Default)
        {
            requestedSize = MathF.Ceiling(requestedSize * CurrentScale);
        }

        var font = _fontsManager.GetFont(desc.FontFamily ?? "Default", requestedSize);

        float scale = AttachedView.Scaling == TextScaling.Pixel
            ? CurrentScale
            : requestedSize / MathF.Max(1f, font.Size);

        return (font, scale);
    }

    private static MarkdownStyle BlockTypeToStyle(MarkdownBlockType type) => type switch
    {
        MarkdownBlockType.Header1  => MarkdownStyle.Header1,
        MarkdownBlockType.Header2  => MarkdownStyle.Header2,
        MarkdownBlockType.Header3  => MarkdownStyle.Header3,
        MarkdownBlockType.Quote    => MarkdownStyle.Quote,
        MarkdownBlockType.Code     => MarkdownStyle.Code,
        _                          => MarkdownStyle.Paragraph,
    };

    // ── Layout ────────────────────────────────────────────────────────────────

    private void LayoutContent(float maxWidth)
    {
        _lastLayoutWidth = maxWidth;
        _layoutLines.Clear();
        _totalHeight = 0;

        if (maxWidth <= 0) return;

        var y = 0f;
        var linePieces = new List<LayoutPiece>();
        var currentX = 0f;
        var lineHeight = 0f;

        var lineSpacing = AttachedView.LineSpacing >= 0f ? AttachedView.LineSpacing : 0f;

        void FinishLine(float extraSpacing = 0f)
        {
            if (linePieces.Count > 0 || lineHeight > 0)
            {
                var hasImage = linePieces.Exists(p => p.Texture != null);
                _layoutLines.Add(new LayoutLine
                {
                    Y = y,
                    Height = lineHeight,
                    Width = currentX,
                    Pieces = new List<LayoutPiece>(linePieces),
                });
                y += lineHeight + (hasImage ? 0f : extraSpacing);
                linePieces.Clear();
                currentX = 0f;
                lineHeight = 0f;
            }
        }

        void LayoutSpanWords(string spanText, IFont font, float fontScale, float spanLineH)
        {
            var lines = spanText.Split('\n');

            for (var li = 0; li < lines.Length; li++)
            {
                var pieceStartX = currentX;
                var pendingText = "";

                var wordParts = lines[li].Split(' ');

                foreach (var wordPart in wordParts)
                {
                    if (wordPart.Length == 0) continue;

                    var joinText = pendingText.Length > 0 ? pendingText + " " + wordPart : wordPart;
                    var joinWidth = font.TextSize((TextSource)NormalizeSpaces(joinText)).Width * fontScale;

                    if (pieceStartX + joinWidth > maxWidth + 0.5f && currentX > 0)
                    {
                        if (pendingText.Length > 0)
                        {
                            var pw = font.TextSize((TextSource)NormalizeSpaces(pendingText)).Width * fontScale;
                            linePieces.Add(new LayoutPiece { X = pieceStartX, Width = pw, Text = NormalizeSpaces(pendingText), Font = font, FontScale = fontScale });
                            pendingText = "";
                        }
                        lineHeight = MathF.Max(lineHeight, spanLineH);
                        FinishLine(lineSpacing);
                        pieceStartX = 0;

                        pendingText = wordPart;
                        currentX = font.TextSize((TextSource)NormalizeSpaces(wordPart)).Width * fontScale;
                    }
                    else
                    {
                        pendingText = joinText;
                        currentX = pieceStartX + joinWidth;
                    }

                    lineHeight = MathF.Max(lineHeight, spanLineH);
                }

                if (pendingText.Length > 0)
                {
                    var pw = font.TextSize((TextSource)NormalizeSpaces(pendingText)).Width * fontScale;
                    linePieces.Add(new LayoutPiece { X = pieceStartX, Width = pw, Text = NormalizeSpaces(pendingText), Font = font, FontScale = fontScale });
                    currentX = pieceStartX + pw;
                }

                // Explicit newline between lines (not after last)
                if (li < lines.Length - 1)
                {
                    lineHeight = MathF.Max(lineHeight, spanLineH);
                    FinishLine(lineSpacing);
                }
            }
        }

        void LayoutSpanImage(string imagePath, bool includeImageHeightToLineHeight)
        {
            var texture = GetOrLoadTexture(imagePath);
            if (texture == null) return;

            var texW = (float)texture.Size.Width;
            var texH = (float)texture.Size.Height;

            var s = AttachedView.Scaling != TextScaling.None ? CurrentScale : 1f;
            var dispW = texW * s;
            var dispH = texH * s;

            if (dispW > maxWidth)
            {
                s = maxWidth / texW;
                dispW = maxWidth;
                dispH = texH * s;
            }

            if (currentX + dispW > maxWidth + 0.5f && currentX > 0)
            {
                FinishLine(lineSpacing);
            }

            linePieces.Add(new LayoutPiece
            {
                X = currentX,
                Width = dispW,
                Texture = texture,
                TextureSize = new SizeF(dispW, dispH),
            });
            currentX += dispW;

            if (includeImageHeightToLineHeight)
            {
                lineHeight = MathF.Max(lineHeight, dispH);
            }
        }

        for (var blockIdx = 0; blockIdx < _blocks.Count; blockIdx++)
        {
            var block = _blocks[blockIdx];

            if (block.Type == MarkdownBlockType.Image)
            {
                var texture = GetOrLoadTexture(block.ImagePath);
                if (texture != null)
                {
                    var texW = (float)texture.Size.Width;
                    var texH = (float)texture.Size.Height;
                    var s = AttachedView.Scaling != TextScaling.None ? CurrentScale : 1f;
                    var dispW = texW * s;
                    var dispH = texH * s;
                    if (dispW > maxWidth) { s = maxWidth / texW; dispW = maxWidth; dispH = texH * s; }

                    _layoutLines.Add(new LayoutLine
                    {
                        Y = y,
                        Height = dispH,
                        Pieces =
                        {
                            new LayoutPiece
                            {
                                X = 0,
                                Width = dispW,
                                Texture = texture,
                                TextureSize = new SizeF(dispW, dispH),
                            }
                        },
                    });
                    y += dispH;
                }

                y += AttachedView.ParagraphSpacing >= 0f ? AttachedView.ParagraphSpacing : block.MarginBottom;
                continue;
            }

            if (block.Type == MarkdownBlockType.Rule)
            {
                _layoutLines.Add(new LayoutLine { Y = y, Height = 2f });
                y += 2f + (AttachedView.ParagraphSpacing >= 0f ? AttachedView.ParagraphSpacing : block.MarginBottom);
                continue;
            }

            // Text block
            var blockStyle = BlockTypeToStyle(block.Type);
            var blockStartLineIdx = _layoutLines.Count;
            var yBeforeBlock = y;

            foreach (var span in block.Spans)
            {
                if (span.ImagePath != null)
                {
                    LayoutSpanImage(span.ImagePath, false);
                    continue;
                }

                var style = span.Style == MarkdownStyle.Paragraph ? blockStyle : span.Style;
                var (font, fontScale) = GetStyledFont(style);
                var spanLineH = font.LineSize * fontScale;

                if (block.Type == MarkdownBlockType.Code)
                {
                    var codeLines = span.Text.Split('\n');
                    for (var cli = 0; cli < codeLines.Length; cli++)
                    {
                        var codeLine = codeLines[cli];
                        if (codeLine.Length > 0)
                        {
                            var cw = font.TextSize((TextSource)codeLine, TextSpacing.Normal).Width * fontScale;
                            linePieces.Add(new LayoutPiece { X = 0, Width = cw, Text = codeLine, Font = font, FontScale = fontScale });
                        }
                        lineHeight = MathF.Max(lineHeight, spanLineH);
                        FinishLine(cli < codeLines.Length - 1 ? lineSpacing : 0f);
                    }
                }
                else
                {
                    LayoutSpanWords(span.Text, font, fontScale, spanLineH);
                }
            }

            FinishLine();

            if (_layoutLines.Count > 0)
                _layoutLines[^1].IsLastInBlock = true;

            // Equalize all line heights within this block to the maximum.
            var blockLineCount = _layoutLines.Count - blockStartLineIdx;
            if (blockLineCount > 1)
            {
                var maxH = 0f;
                for (var li = blockStartLineIdx; li < _layoutLines.Count; li++)
                    maxH = MathF.Max(maxH, _layoutLines[li].Height);

                var newY = yBeforeBlock;
                for (var li = blockStartLineIdx; li < _layoutLines.Count; li++)
                {
                    _layoutLines[li].Y = newY;
                    _layoutLines[li].Height = maxH;
                    newY += maxH + (_layoutLines[li].IsLastInBlock ? 0f : lineSpacing);
                }
                y = newY;
            }

            y += AttachedView.ParagraphSpacing >= 0f ? AttachedView.ParagraphSpacing : block.MarginBottom;
        }

        _totalHeight = y;
    }

    // ── Image loading ─────────────────────────────────────────────────────────

    private ITexture GetOrLoadTexture(string path)
    {
        if (path == null) return null;
        if (_textures.TryGetValue(path, out var cached)) return cached.Resource;

        try
        {
            var resourcePath = AttachedView?.Mapper?.GetImagePath(path) ?? path;
            var handle = _contentManager.Get<ITexture>(resourcePath);
            _textures[path] = handle;
            return handle.Resource;
        }
        catch
        {
            return null;
        }
    }

    private static string NormalizeSpaces(string s) => s.Replace('\u00A0', ' ');

    // ── Markdown parser ───────────────────────────────────────────────────────

    private static List<MarkdownBlock> ParseMarkdown(string text)
    {
        var blocks = new List<MarkdownBlock>();
        if (string.IsNullOrEmpty(text)) return blocks;

        var lines = text.Split('\n');
        var i = 0;

        while (i < lines.Length)
        {
            var line = lines[i].TrimEnd('\r');

            if (string.IsNullOrWhiteSpace(line))
            {
                i++;
                continue;
            }

            if (line.StartsWith("### "))
            {
                blocks.Add(CreateTextBlock(MarkdownBlockType.Header3, line.Substring(4), 4f));
                i++;
                continue;
            }

            if (line.StartsWith("## "))
            {
                blocks.Add(CreateTextBlock(MarkdownBlockType.Header2, line.Substring(3), 6f));
                i++;
                continue;
            }

            if (line.StartsWith("# "))
            {
                blocks.Add(CreateTextBlock(MarkdownBlockType.Header1, line.Substring(2), 8f));
                i++;
                continue;
            }

            if (line == "---" || line == "***" || line == "___")
            {
                blocks.Add(new MarkdownBlock { Type = MarkdownBlockType.Rule, MarginBottom = 4f });
                i++;
                continue;
            }

            if (line.StartsWith("> "))
            {
                var sb = new System.Text.StringBuilder();
                while (i < lines.Length)
                {
                    var l = lines[i].TrimEnd('\r');
                    if (!l.StartsWith("> ")) break;
                    if (sb.Length > 0) sb.Append(' ');
                    sb.Append(l.Substring(2));
                    i++;
                }
                blocks.Add(CreateTextBlock(MarkdownBlockType.Quote, sb.ToString(), 4f));
                continue;
            }

            if (line.StartsWith("```"))
            {
                i++;
                var codeSb = new System.Text.StringBuilder();
                while (i < lines.Length)
                {
                    var l = lines[i].TrimEnd('\r');
                    i++;
                    if (l.StartsWith("```")) break;
                    if (codeSb.Length > 0) codeSb.Append('\n');
                    codeSb.Append(l);
                }
                var codeBlock = new MarkdownBlock { Type = MarkdownBlockType.Code, MarginBottom = 4f };
                codeBlock.Spans.Add(new InlineSpan { Text = codeSb.ToString(), Style = MarkdownStyle.Code });
                blocks.Add(codeBlock);
                continue;
            }

            // Standalone image: ![alt](path)
            if (line.StartsWith("![") && line.Contains("](") && line.EndsWith(")"))
            {
                var pathStart = line.IndexOf("](", StringComparison.Ordinal) + 2;
                var path = line.Substring(pathStart, line.Length - pathStart - 1);
                blocks.Add(new MarkdownBlock { Type = MarkdownBlockType.Image, ImagePath = path, MarginBottom = 4f });
                i++;
                continue;
            }

            // Paragraph: accumulate until blank line or another block element
            {
                var sb = new System.Text.StringBuilder();
                while (i < lines.Length)
                {
                    var l = lines[i].TrimEnd('\r');
                    if (string.IsNullOrWhiteSpace(l)) break;
                    if (l.StartsWith("#") || l.StartsWith(">") || l.StartsWith("```") ||
                        (l.StartsWith("![") && l.Contains("](") && l.EndsWith(")")) ||
                        l == "---" || l == "***" || l == "___") break;
                    if (sb.Length > 0) sb.Append(' ');
                    sb.Append(l);
                    i++;
                }
                var block = new MarkdownBlock { Type = MarkdownBlockType.Paragraph, MarginBottom = 4f };
                block.Spans.AddRange(ParseInline(sb.ToString()));
                if (block.Spans.Exists(s => s.ImagePath != null))
                    block.Type = MarkdownBlockType.InlineImage;
                blocks.Add(block);
            }
        }

        return blocks;
    }

    private static MarkdownBlock CreateTextBlock(MarkdownBlockType type, string text, float marginBottom)
    {
        var block = new MarkdownBlock { Type = type, MarginBottom = marginBottom };
        block.Spans.AddRange(ParseInline(text));
        return block;
    }

    private static List<InlineSpan> ParseInline(string text)
    {
        var spans = new List<InlineSpan>();
        if (string.IsNullOrEmpty(text)) return spans;

        var i = 0;
        var sb = new System.Text.StringBuilder();

        while (i < text.Length)
        {
            // Bold-italic: ***
            if (i + 2 < text.Length && text[i] == '*' && text[i + 1] == '*' && text[i + 2] == '*')
            {
                FlushSpan(spans, sb);
                i += 3;
                var end = text.IndexOf("***", i, StringComparison.Ordinal);
                if (end < 0) { sb.Append("***"); continue; }
                spans.Add(new InlineSpan { Text = text.Substring(i, end - i), Style = MarkdownStyle.BoldItalic });
                i = end + 3;
                continue;
            }

            // Bold: **
            if (i + 1 < text.Length && text[i] == '*' && text[i + 1] == '*')
            {
                FlushSpan(spans, sb);
                i += 2;
                var end = text.IndexOf("**", i, StringComparison.Ordinal);
                if (end < 0) { sb.Append("**"); continue; }
                spans.Add(new InlineSpan { Text = text.Substring(i, end - i), Style = MarkdownStyle.Bold });
                i = end + 2;
                continue;
            }

            // Italic: *
            if (text[i] == '*')
            {
                FlushSpan(spans, sb);
                i += 1;
                var end = text.IndexOf('*', i);
                if (end < 0) { sb.Append('*'); continue; }
                spans.Add(new InlineSpan { Text = text.Substring(i, end - i), Style = MarkdownStyle.Italic });
                i = end + 1;
                continue;
            }

            // Inline code: `
            if (text[i] == '`')
            {
                FlushSpan(spans, sb);
                i += 1;
                var end = text.IndexOf('`', i);
                if (end < 0) { sb.Append('`'); continue; }
                spans.Add(new InlineSpan { Text = text.Substring(i, end - i), Style = MarkdownStyle.Code });
                i = end + 1;
                continue;
            }

            // HTML entity: &nbsp;
            if (text[i] == '&' && i + 5 < text.Length && text.Substring(i, 6) == "&nbsp;")
            {
                sb.Append('\u00A0');
                i += 6;
                continue;
            }

            // Inline image: ![alt](path)
            if (text[i] == '!' && i + 1 < text.Length && text[i + 1] == '[')
            {
                var closeB = text.IndexOf(']', i + 2);
                if (closeB > i + 2 && closeB + 1 < text.Length && text[closeB + 1] == '(')
                {
                    var closeP = text.IndexOf(')', closeB + 2);
                    if (closeP > closeB + 2)
                    {
                        FlushSpan(spans, sb);
                        var imgPath = text.Substring(closeB + 2, closeP - closeB - 2);
                        spans.Add(new InlineSpan { ImagePath = imgPath, Style = MarkdownStyle.Paragraph });
                        i = closeP + 1;
                        continue;
                    }
                }
            }

            // Link: [text](url) — renders link text with Link style, ignores URL
            if (text[i] == '[')
            {
                var closeB = text.IndexOf(']', i);
                if (closeB > i && closeB + 1 < text.Length && text[closeB + 1] == '(')
                {
                    var closeP = text.IndexOf(')', closeB + 2);
                    if (closeP > closeB + 2)
                    {
                        FlushSpan(spans, sb);
                        var linkText = text.Substring(i + 1, closeB - i - 1);
                        spans.Add(new InlineSpan { Text = linkText, Style = MarkdownStyle.Link });
                        i = closeP + 1;
                        continue;
                    }
                }
            }

            sb.Append(text[i]);
            i++;
        }

        FlushSpan(spans, sb);
        return spans;
    }

    private static void FlushSpan(List<InlineSpan> spans, System.Text.StringBuilder sb)
    {
        if (sb.Length > 0)
        {
            spans.Add(new InlineSpan { Text = sb.ToString(), Style = MarkdownStyle.Paragraph });
            sb.Clear();
        }
    }
}
