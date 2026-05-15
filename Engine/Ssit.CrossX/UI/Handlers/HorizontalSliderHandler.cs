using System;
using System.Numerics;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class HorizontalSliderHandler<TSlider> : ViewHandler<TSlider>, IUiCommandHandler where TSlider: HorizontalSlider, new()
{
    private readonly IPaletteSource _paletteSource;
    private readonly ResourceHandle<DualTexture> _texture;
    private readonly IColorSource _colorSource;
    
    public HorizontalSliderHandler(CreateHandlerParameters parameters, IContentManager contentManager, IHandlerMapper handlerMapper, IPaletteSource paletteSource = null) : base(parameters)
    {
        _paletteSource = paletteSource;
        _texture = contentManager.Get<DualTexture>(AttachedView.Template.Path);
        _colorSource = parameters.Parent?.GetParent<IColorSource>(true);
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _texture?.Dispose();
    }

    public override void CalculateSize(out Length width, out Length height)
    {
        width = AttachedView.Width ?? Length.Fill;
        height = AttachedView.Height ?? Length.Auto;
        
        if (height.IsAuto)
        {
            height = new Length(AttachedView.Template.Thumb.Height);
        }

        if (width.IsAuto)
        {
            throw new NotSupportedException("HorizontalSlider width cannot be auto!");
        }
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        var sb = ScreenBounds;

        var offset = AttachedView.Value.Value - AttachedView.Min;
        var floatOffset = (float) offset / (AttachedView.Max - AttachedView.Min);
        
        var thumbSize = AttachedView.Template.Thumb.Size.ToVector() * CurrentScale;
        var totalWidth = sb.Width - thumbSize.X;

        var fgColor =
            AttachedView.ForegroundColor?.GetColor(_paletteSource, renderer, _colorSource) ?? RgbaColor.White;
        
        var outlineColor =
            AttachedView.OutlineColor?.GetColor(_paletteSource, renderer, _colorSource) ?? RgbaColor.Black;
        
        var thumbRect = new RectangleF(sb.X + floatOffset * totalWidth, sb.Center.Y - thumbSize.Y / 2, thumbSize.X, thumbSize.Y);

        var posX = sb.X;
        var posY = sb.Center.Y - AttachedView.Template.Track.Height * CurrentScale / 2f;
        
        renderer.SpriteRenderer.Draw(_texture.Resource.Texture, new Vector2(posX, posY), AttachedView.Template.TrackStart, origin: Vector2.Zero, scale: CurrentScale, color: fgColor); 
        renderer.SpriteRenderer.Draw(_texture.Resource.Outline, new Vector2(posX, posY), AttachedView.Template.TrackStart, origin: Vector2.Zero, scale: CurrentScale, color: outlineColor);
        
        posX += AttachedView.Template.TrackStart.Width * CurrentScale;
        
        var trackLength = sb.Width - (AttachedView.Template.TrackStart.Width + AttachedView.Template.TrackEnd.Width) * CurrentScale;
        
        renderer.SpriteRenderer.Draw(_texture.Resource.Texture, new RectangleF(posX, posY, trackLength, AttachedView.Template.Track.Height * CurrentScale), AttachedView.Template.Track, nullableColor: fgColor); 
        renderer.SpriteRenderer.Draw(_texture.Resource.Outline, new RectangleF(posX, posY, trackLength, AttachedView.Template.Track.Height * CurrentScale), AttachedView.Template.Track, nullableColor: outlineColor);

        posX += trackLength;
        
        renderer.SpriteRenderer.Draw(_texture.Resource.Texture, new Vector2(posX, posY), AttachedView.Template.TrackEnd, origin: Vector2.Zero, scale: CurrentScale, color: fgColor); 
        renderer.SpriteRenderer.Draw(_texture.Resource.Outline, new Vector2(posX, posY), AttachedView.Template.TrackEnd, origin: Vector2.Zero, scale: CurrentScale, color: outlineColor);
        
        renderer.SpriteRenderer.Draw(_texture.Resource.Texture, thumbRect, AttachedView.Template.Thumb, nullableColor: fgColor);
        renderer.SpriteRenderer.Draw(_texture.Resource.Outline, thumbRect, AttachedView.Template.Thumb, nullableColor: outlineColor);
    }

    public bool OnUiButton(UiButton button, IInputContext context)
    {
        switch (button)
        {
            case UiButton.Left:
                if (AttachedView?.Value is not null)
                {
                    var value = AttachedView.Value.Value;
                    value = Math.Max(AttachedView.Min, Math.Min(value - 1, AttachedView.Max));
                    AttachedView.Value.Value = value;
                    return true;
                }
                break;
            
            case UiButton.Right:
                if (AttachedView?.Value is not null)
                {
                    var value = AttachedView.Value.Value;
                    value = Math.Max(AttachedView.Min, Math.Min(value + 1, AttachedView.Max));
                    AttachedView.Value.Value = value;
                    return true;
                }
                break;
        }
        return false;
    }
}