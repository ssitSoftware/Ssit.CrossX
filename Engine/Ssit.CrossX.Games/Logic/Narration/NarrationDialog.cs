using System.Collections.Generic;

namespace Ssit.CrossX.Games.Logic.Narration;

internal class NarrationDialog
{
    public IReadOnlySet<string> On { get; }
    public bool Highlight { get; }
    public NarrationEntry Entry { get; }
    public string DefaultLanguage { get; }

    public NarrationDialog(IReadOnlySet<string> on, bool highlight, string defaultLanguage, NarrationEntry entry)
    {
        On = on;
        Highlight = highlight;
        DefaultLanguage = defaultLanguage;
        Entry = entry;
    }
}