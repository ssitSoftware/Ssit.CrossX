using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.Common.Services;

public interface ITranslator
{
    SharedString this[string key] { get; }
    void ToggleLanguage();
    int CurrentLanguage { get; set; }
}