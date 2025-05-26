using System;
using System.Threading.Tasks;

namespace Ssit.CrossX.Games.Logic.Narration;

public interface INarrationSystem
{
    event Action<string> NarrationAction;
    Task StartNarration(string subject);
    bool HasRequest(string subject);
    bool HasNarration(string subject);
}