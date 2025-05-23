using System;
using System.Threading.Tasks;

namespace Ssit.CrossX.Games.Logic;

public interface IGameDialogs
{
    void Show(string text, string[] replyOptions, Action<int> onReply = null);
    Task<int> ShowAsync(string text, string[] replyOptions);
    bool IsConversationActive { get; }
}