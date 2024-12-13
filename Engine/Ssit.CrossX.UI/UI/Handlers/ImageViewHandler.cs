using System;
using System.Drawing;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Parameters;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class ImageViewHandler : BackgroundHandler<ImageView>
{
    private readonly IIoCContainer _container;

    private ImageScalingMode ScalingMode => AttachedView.Scaling ?? ImageScalingMode.None;
    private ContentAlign ContentAlign => AttachedView.ContentAlign ?? ContentAlign.Center | ContentAlign.VCenter;
    
    public ImageViewHandler(CreateHandlerParameters parameters, IIoCContainer container) : base(parameters)
    {
        if (AttachedView.Source is null)
        {
            throw new InvalidOperationException("The image view is missing a Source.");
        }

        _container = container;

        AttachedView.Source.ImageChanged += OnImageChanged;
    }

    private void OnImageChanged()
    {
        Parent?.RecalculateLayout(AttachedView);
    }

    public override void CalculateAlign(out Align horizontalAlign, out Align verticalAlign)
    {
        if (AttachedView.HorizontalAlign.HasValue)
        {
            horizontalAlign = AttachedView.HorizontalAlign.Value;
        }
        else
        {
            switch (ScalingMode)
            {
                case ImageScalingMode.None:
                    horizontalAlign = Align.Center;
                    break;
                
                default:
                    horizontalAlign = Align.Fill;
                    break;
            }
        }
        
        if (AttachedView.VerticalAlign.HasValue)
        {
            verticalAlign = AttachedView.VerticalAlign.Value;
        }
        else
        {
            switch (ScalingMode)
            {
                case ImageScalingMode.None:
                    verticalAlign = Align.Center;
                    break;
                
                default:
                    verticalAlign = Align.Fill;
                    break;
            }
        }
    }

    public override void CalculateSize(out Length width, out Length height)
    {
        width = AttachedView.Width ?? Length.Auto;
        height = AttachedView.Height ?? Length.Auto;

        if (width.IsAuto || height.IsAuto)
        {
            var texture = AttachedView.Source?.GetTexture(_container);

            if (width.IsAuto)
            {
                if ((AttachedView.Transform ?? ImageTransform.None) is ImageTransform.Rotate90 or ImageTransform.Rotate270)
                {
                    width = texture?.Resource.Size.Height ?? 0;
                }
                else
                {
                    width = texture?.Resource.Size.Width ?? 0;
                }
            }

            if (height.IsAuto)
            {
                if ((AttachedView.Transform ?? ImageTransform.None) is ImageTransform.Rotate90 or ImageTransform.Rotate270)
                {
                    height = texture?.Resource.Size.Width ?? 0;
                }
                else
                {
                    height = texture?.Resource.Size.Height ?? 0;
                }
            }
        }
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        AttachedView.Source.ImageChanged += OnImageChanged;
    }

    public override void Draw(IRenderer renderer)
    {
        base.Draw(renderer);
        var texture = AttachedView.Source?.GetTexture(_container);
        
        if (texture is null)
        {
            return;
        }

        CalculateTargetRects(texture.Resource, out var targetRect, out var sourceRect);
        renderer.DrawTexture(texture.Resource, targetRect, sourceRect, AttachedView?.TintColor, AttachedView?.Transform ?? ImageTransform.None);
    }

    private void CalculateTargetRects(ITexture texture, out RectangleF targetRect, out Rectangle sourceRect)
    {
        var size = texture.Size;
        if ((AttachedView.Transform ?? ImageTransform.None) is ImageTransform.Rotate90 or ImageTransform.Rotate270)
        {
            size = new Size(size.Height, size.Width);
        }

        var targetSize = new SizeF(size.Width, size.Height);
        
        switch (AttachedView.Scaling)
        {
            case ImageScalingMode.AspectFit:
            {
                var sb = ScreenBounds;
                var scale = MathF.Min(sb.Width / size.Width, sb.Height / size.Height);

                var width = size.Width * scale;
                var height = size.Height * scale;
                
                targetSize = new SizeF(width, height);
            }
            break;
            
            case ImageScalingMode.AspectFill:
            {
                var sb = ScreenBounds;
                var scale = MathF.Max(sb.Width / size.Width, sb.Height / size.Height);

                var width = size.Width * scale;
                var height = size.Height * scale;
                
                targetSize = new SizeF(width, height);
            }
            break;
            
            case ImageScalingMode.Fill:
                targetSize = ScreenBounds.Size;
                break;
            
             case ImageScalingMode.None: 
                targetSize = new SizeF(size.Width, size.Height);
                break;
        }
        
        
        
        sourceRect = new Rectangle(0, 0, size.Width, size.Height);
        if ((AttachedView.Transform ?? ImageTransform.None) is ImageTransform.Rotate90 or ImageTransform.Rotate270)
        {
            sourceRect = new Rectangle(sourceRect.Y, sourceRect.X, sourceRect.Height, sourceRect.Width);
        }

        targetRect = ScreenBounds;
    }
}