using System;
using System.Threading.Tasks;

namespace Ssit.CrossX.Games.Logic;

public interface INarrationSystem
{
    event Action<string> OnNarrationAction;
    
    Task StartNarration(string subject);
    bool HasRequest(string subject);
}