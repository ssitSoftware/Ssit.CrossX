using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Ssit.CrossX.UI.Values;

namespace RetroGunslinger.Core.Game;

public interface IGameDialogs
{
    event Action<int> FocusElement;
    
    ICommand ReplyCommand { get; }
    SharedBool Visible { get; }
    SharedString CurrentText { get; }
    
    SharedBool[] ReplyOptionVisible { get; }
    SharedString[] ReplyOptions { get; }
    
    void Show(string text, string[] replyOptions, Action<int> onReply = null);
    Task<int> ShowAsync(string text, string[] replyOptions);
}