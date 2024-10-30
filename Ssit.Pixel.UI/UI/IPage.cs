namespace Ssit.Pixel.UI;

internal interface IPage
{
    void Load(IStylesManager styles, object viewModel);
}