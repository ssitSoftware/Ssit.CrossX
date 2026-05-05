namespace Ssit.CrossX.UI.Handlers;

public interface IColorSource: IViewParent
{
    RgbaColor? GetColor(string id);
}