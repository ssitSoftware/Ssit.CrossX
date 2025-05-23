using System;
using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.UI.Values;

namespace RetroGunslinger.Core.Game;

public class GameDialogs : GameDialogsBase, IGameDialogsUi
{
    public event Action<int> FocusElement;    
    public SharedBool Visible => _visible;
    public SharedString CurrentText => _currentText;
    public SharedBool[] ReplyOptionVisible => _replyOptionVisible;
    public SharedString[] ReplyOptions => _replyOptions;

    public override bool IsConversationActive => Visible.Value;
    
    public ICommand ReplyCommand => _replyCommand;
    
    private readonly SharedBoolMutable _visible = new(false);
    private readonly SharedStringValue _currentText = new("");

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

    public GameDialogs(IActionScheduler actionScheduler): base(actionScheduler)
    {
        _replyCommand = new SyncCommand(OnCommandReply, CanReply);
    }

    private bool CanReply(object arg)
    {
        if (arg is int index)
        {
            return _replyOptionVisible[index].Value;
        }

        return false;
    }

    private void OnCommandReply(object obj)
    {
        ShouldHide = true;

        ActionScheduler.Schedule(() =>
        {
            if (!ShouldHide)
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
                    OnReply(0);
                    break;
                
                case 2:
                    OnReply(index > 0 ? 1 : 0);
                    break;
                
                case 3:
                    OnReply(index);
                    break;
            }
        }
        else
        {
            OnReply(-1);
        }
    }

    protected override void SetValuesForDialog(string text, string[] replyOptions)
    {
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
        
        _visible.SetValue(true);
        
        _currentText.SetText("");
        _currentText.SetText(text);
        
        FocusElement?.Invoke(focus);
        
        _replyCommand.RaiseCanExecuteChanged();
    }
}