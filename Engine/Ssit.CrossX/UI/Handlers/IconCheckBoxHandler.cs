using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.Input;
using Ssit.CrossX.UI.Common.Pages;
using Ssit.CrossX.UI.Handlers.Helpers;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Values;
using Ssit.CrossX.UI.Views;
using Ssit.IoC;

namespace Ssit.CrossX.UI.Handlers;

public class IconCheckBoxHandler<TCheckBox> : BackgroundHandler<TCheckBox>, IInputConsumer, IFocusable where TCheckBox: IconCheckBox
{
    private readonly IIoCContainer _container;
    public string UniqueId => AttachedView?.UniqueId;
    public bool SkipNavigation => false;
    public bool Enabled => _buttonHelper.IsEnabled;
    public bool DisableAllInput => _buttonHelper.IsExecutingCommand;
    
    public bool Focused { get; private set; }
    
    private readonly ButtonHelper<TCheckBox, IconCheckBoxHandler<TCheckBox>> _buttonHelper;

    public IconCheckBoxHandler(CreateHandlerParameters parameters, IPaletteSource paletteSource, IUiSounds uiSounds,
        IIoCContainer container, IHapticDevice hapticDevice, PageInputContext pageInputContext) : base(parameters, paletteSource)
    {
        _container = container;
        _buttonHelper = new ButtonHelper<TCheckBox, IconCheckBoxHandler<TCheckBox>>(this, AttachedView?.CustomSounds ?? uiSounds, hapticDevice, pageInputContext);
    }

    public void ProcessHover(Vector2? hoverPosition, int? matchingPointerId, IInputContext context) => _buttonHelper.ProcessHover(hoverPosition, matchingPointerId, context);

    public bool ProcessInput(IReadOnlyList<Pointer> pointers, IInputContext context) => _buttonHelper.ProcessInput(pointers, context);

    public void CancelPointer(int pointerId, IInputContext context) => _buttonHelper.CancelPointer(pointerId, context);
    
    public bool OnUiButton(UiButton button, IInputContext context)
    {
        try
        {
            return _buttonHelper.OnUiButton(button, context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Debugger.Break();
        }

        return false;
    }

    public void SetFocus()
    {
        Focused = true;
    }

    public bool ResetFocus()
    {
        Focused = false;
        return true;
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        base.OnDraw(renderer);
        
        var source = _buttonHelper.IsPressed ? AttachedView.ImagePushed : AttachedView.Image;
        source ??= AttachedView.Image;
        
        var texture = source?.GetTexture(_container);
        
        if (texture is null)
        {
            return;
        }
        
        var targetSize = ((SizeF)AttachedView.IconSize) * CurrentScale;
        var targetRect = ScreenBounds;
        
        var pos = targetRect.TopLeft;
        
        pos.X += (targetRect.Width - targetSize.Width) / 2;
        pos.Y += (targetRect.Height - targetSize.Height) / 2;
        
        targetRect = new RectangleF(pos, targetSize);
        
        var iw = AttachedView.IconSize.Width;
        var ih = AttachedView.IconSize.Height;

        var frameIndex = AttachedView.IsChecked.Value ? AttachedView.FrameOn : AttachedView.FrameOff;
        
        var sourceRect = new Rectangle(frameIndex * iw, 0, iw, ih);
        renderer.SpriteRenderer.Draw(texture.Resource, targetRect, sourceRect);
    }
}