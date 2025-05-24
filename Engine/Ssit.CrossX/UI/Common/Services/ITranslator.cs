using System;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.UI.Common.Services;

public interface ITranslator
{
    event Action LanguageChanged;
    SharedString this[string key] { get; }
    void ToggleLanguage(bool previous = false);
    int CurrentLanguage { get; set; }
}