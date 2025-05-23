using System;
using System.Threading.Tasks;

namespace Ssit.CrossX.Games.Logic.Narration;

public interface INarrationSystem
{
    event Action<string> NarrationAction;
    event Action NarrationUpdated;
    Task StartNarration(string subject);
    bool HasRequest(string subject);
}