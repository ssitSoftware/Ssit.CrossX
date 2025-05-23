using System.Collections.Generic;

namespace Ssit.CrossX.Games.Logic.Narration;

internal class NarrationDialog
{
    public IReadOnlySet<string> On { get; }
    public bool Highlight { get; }
    public NarrationEntry Entry { get; }
    
    public NarrationDialog(IReadOnlySet<string> on, bool highlight, NarrationEntry entry)
    {
        On = on;
        Highlight = highlight;
        Entry = entry;
    }
}