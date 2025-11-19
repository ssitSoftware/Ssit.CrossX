using System;
using System.Threading.Tasks;

namespace Ssit.CrossX.XxGames.Logic.Narration;

public interface INarrationSystem
{
    event Action<string> NarrationAction;
    Task StartNarration(string subject);
    bool HasRequest(string subject);
    bool HasNarration(string subject);
    INarrationSystem SetValue(string key, string value);
}