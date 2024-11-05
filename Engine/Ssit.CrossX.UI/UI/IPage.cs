using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI;

internal interface IPage
{
    void Load(IUiServices services, object viewModel);
}