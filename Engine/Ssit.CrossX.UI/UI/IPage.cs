using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI;

internal interface IPage
{
    void Load(RectangleF bounds, IUiServices services, object viewModel);
    void Update(float dt);
    void Draw(IRenderer renderer);
    void SetBounds(RectangleF bounds);
}