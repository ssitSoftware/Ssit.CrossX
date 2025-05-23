namespace Ssit.CrossX.Games.Logic.Narration;

internal class NarrationEntry
{
    public string Text { get; }
    public NarrationOption[] Options { get; }

    public NarrationEntry(string text, NarrationOption[] options)
    {
        Text = text;
        Options = options;
    }
}