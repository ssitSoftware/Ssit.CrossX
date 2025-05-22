using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Values;

namespace RetroGunslinger.Core.Game;

public class GameDialogs : IGameDialogs
{
    private readonly IActionScheduler _actionScheduler;
    public event Action<int> FocusElement;
    
    public SharedBool Visible => _visible;
    public SharedString CurrentText => _currentText;
    public SharedBool[] ReplyOptionVisible => _replyOptionVisible;
    public SharedString[] ReplyOptions => _replyOptions;

    public ICommand ReplyCommand => _replyCommand;

    private Action<int> _onReply;
    private bool _shouldHide;
    
    private readonly SharedBoolMutable _visible = new SharedBoolMutable(false);
    private readonly SharedStringValue _currentText = new SharedStringValue("");

    private int _visibleReplays = 0;
    
    private readonly SharedBoolMutable[] _replyOptionVisible =
    [
        new(false),
        new(false),
        new(false)
    ];

    private readonly SharedStringValue[] _replyOptions =
    [
        new(""),
        new(""),
        new("")
    ];

    private SyncCommand _replyCommand;

    public GameDialogs(IActionScheduler actionScheduler)
    {
        _actionScheduler = actionScheduler;
        _replyCommand = new SyncCommand(OnReply, CanReply);
    }

    private bool CanReply(object arg)
    {
        if (arg is int index)
        {
            return _replyOptionVisible[index].Value;
        }

        return false;
    }

    private void OnReply(object obj)
    {
        _shouldHide = true;

        _actionScheduler.Schedule(() =>
        {
            if (!_shouldHide)
                return;
            
            _replyOptions[0].SetText("");
            _replyOptions[1].SetText("");
            _replyOptions[2].SetText("");

            _replyOptionVisible[0].SetValue(false);
            _replyOptionVisible[1].SetValue(false);
            _replyOptionVisible[2].SetValue(false);

            _currentText.SetText("");
            _visible.SetValue(false);

            _replyCommand.RaiseCanExecuteChanged();
            FocusElement?.Invoke(-1);
        });

        if (obj is int index)
        {
            switch (_visibleReplays)
            {
                case 1:
                    _onReply?.Invoke(0);
                    break;
                
                case 2:
                    _onReply?.Invoke(index > 0 ? 1 : 0);
                    break;
                
                case 3:
                    _onReply?.Invoke(index);
                    break;
            }
        }
        _onReply = null;
    }

    public Task<int> ShowAsync(string text, string[] replyOptions)
    {
        var tcs = new TaskCompletionSource<int>();
        
        _actionScheduler.Schedule( () => Show(text, replyOptions, index =>
        {
            if (!tcs.Task.IsCompleted)
            {
                tcs.SetResult(index);
            }

            _onReply = null;
        }));
        return tcs.Task;
    }

    public void Show(string text, string[] replyOptions, Action<int> onReply)
    {
        _shouldHide = false; 
        _onReply?.Invoke(-1);
        _onReply = null;
        
        _replyOptionVisible[0].SetValue(false);
        _replyOptionVisible[1].SetValue(false);
        _replyOptionVisible[2].SetValue(false);

        int focus = 0;

        _visibleReplays = replyOptions.Length;
        switch (replyOptions.Length)
        {
            case 1:
                _replyOptions[2].SetText(replyOptions[0]);
                _replyOptionVisible[2].SetValue(true);
                focus = 2;
                break;
            
            case 2:
                _replyOptions[0].SetText(replyOptions[0]);
                _replyOptionVisible[0].SetValue(true);
                
                _replyOptions[2].SetText(replyOptions[1]);
                _replyOptionVisible[2].SetValue(true);
                break;
            
            case 3:
                _replyOptions[0].SetText(replyOptions[0]);
                _replyOptionVisible[0].SetValue(true);
                
                _replyOptions[1].SetText(replyOptions[1]);
                _replyOptionVisible[1].SetValue(true);
                
                _replyOptions[2].SetText(replyOptions[2]);
                _replyOptionVisible[2].SetValue(true);
                break;
            
            default:
                throw new InvalidOperationException();
        }

        _onReply = onReply;
        _visible.SetValue(true);
        
        _currentText.SetText("");
        _currentText.SetText(text);
        
        FocusElement?.Invoke(focus);
        
        _replyCommand.RaiseCanExecuteChanged();
    }
}