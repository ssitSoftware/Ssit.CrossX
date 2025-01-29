using Ssit.CrossX.UI.Values;

namespace SampleGame.Services;

public interface ITranslator
{
    SharedString this[string key] { get; }
    void ToggleLanguage();
}