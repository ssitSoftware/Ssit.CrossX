using Ssit.CrossX.Input.Internal;
namespace Ssit.CrossX.NET.Apple.Input;

public class PointingDevicesImpl : PointingDevicesBase
{
#if __MACCATALYST__
    //private readonly MetalKit.MTKView _view;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public PointingDevicesImpl(MetalKit.MTKView view)
    {
        //_view = view;
    }

    public override void SetHoverPosition(System.Numerics.Vector2 position)
    {
        // var pos = _view.ConvertPointToView(new CoreGraphics.CGPoint(position.X, position.Y), null);
        // var screenPos = _view.Window.ConvertPointToWindow(pos, null);
        //
        // var scale = _view.Window.Bounds.Width / _view.Window.Screen.Bounds.Width;
        // screenPos.X /= scale;
        // screenPos.Y /= scale;
        
        //CoreGraphics.CGDisplay.MoveCursor(CoreGraphics.CGDisplay.MainDisplayID, screenPos);
    }
#endif
}