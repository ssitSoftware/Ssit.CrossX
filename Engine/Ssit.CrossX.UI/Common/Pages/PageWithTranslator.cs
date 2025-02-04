using Ssit.CrossX.Common.Services;
using Ssit.CrossX.UI;

namespace Ssit.CrossX.Common.Pages;

public abstract class PageWithTranslator<TViewModel> : Page<TViewModel> where TViewModel : class
{
    private ITranslator _translator;
    protected ITranslator Translator => _translator ??= Services.Get<ITranslator>();
}