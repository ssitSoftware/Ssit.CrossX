using System.Collections.Generic;

namespace Ssit.CrossX.Games.Logic.Narration;

internal class NarrationOption
{
    public Dictionary<string, string> Text { get; }
    public NarrationEntry Entry { get; }
    public string Action { get; }
    public string SetTag { get; }
    
    public NarrationOption(Dictionary<string, string> text, NarrationEntry entry, string action, string setTag)
    {
        Text = text;
        Entry = entry;
        Action = action;
        SetTag = setTag;
    }
}