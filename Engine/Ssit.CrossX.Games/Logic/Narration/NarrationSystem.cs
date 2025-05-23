using System;
using System.Threading.Tasks;

namespace Ssit.CrossX.Games.Logic.Narration;

public class NarrationSystem: INarrationSystem
{
    private readonly IGameDialogs _dialogs;
    public event Action<string> OnNarrationAction;

    public NarrationSystem(IGameDialogs dialogs)
    {
        _dialogs = dialogs;
    }
    
    public Task StartNarration(string subject)
    {
        throw new NotImplementedException();
    }

    public bool HasRequest(string subject)
    {
        throw new NotImplementedException();
    }
}