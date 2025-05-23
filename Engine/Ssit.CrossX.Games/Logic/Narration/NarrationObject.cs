using System.Collections.Generic;

namespace Ssit.CrossX.Games.Logic.Narration;


internal class NarrationObject
{
    public string Name { get; }
    public IReadOnlyList<NarrationDialog> Dialogs { get; }

    public NarrationObject(string name, IReadOnlyList<NarrationDialog> dialogs)
    {
        Name = name;
        Dialogs = dialogs;
    }
}