using Ssit.CrossX.UI.Common.Services;

namespace Ssit.CrossX.UI.Common.Pages;

public abstract class PageWithTranslator<TViewModel> : Page<TViewModel> where TViewModel : class
{
    private ITranslator _translator;
    protected ITranslator Translator => _translator ??= Services.Get<ITranslator>();
}