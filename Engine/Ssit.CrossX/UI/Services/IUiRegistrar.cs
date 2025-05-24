using System;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Services;

public interface IUiRegistrar
{
    IUiRegistrar RegisterTemplates<TTemplate>() where TTemplate : TemplatesContainer;
    IUiRegistrar RegisterStyle<TView>(string name, Action<TView> applyStyle) where TView: View;
    IUiRegistrar RegisterPage<TViewModel, TPage>() where TPage : Page<TViewModel> where TViewModel : class;
}