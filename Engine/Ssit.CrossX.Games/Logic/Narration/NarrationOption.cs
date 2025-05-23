namespace Ssit.CrossX.Games.Logic.Narration;

internal class NarrationOption
{
    public string Text { get; }
    public NarrationEntry Entry { get; }
    public string Action { get; }
    public string SetTag { get; }
    
    public NarrationOption(string text, NarrationEntry entry, string action, string setTag)
    {
        Text = text;
        Entry = entry;
        Action = action;
        SetTag = setTag;
    }
}