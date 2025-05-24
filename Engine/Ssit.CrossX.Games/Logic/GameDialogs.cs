using System;
using System.Collections.Generic;
using System.Windows.Input;
using Ssit.CrossX.Commands;
using Ssit.CrossX.Core;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.Games.Logic;

public class GameDialogs : GameDialogsBase, IGameDialogsUi
{
    public event Action<int> FocusElement;    
    public SharedBool Visible => _visible;
    public SharedString CurrentText => _currentText;
    public IReadOnlyList<SharedBool> ReplyOptionVisible => _replyOptionVisible;
    public IReadOnlyList<SharedString> ReplyOptions => _replyOptions;

    public override bool IsConversationActive => Visible.Value;
    
    public ICommand ReplyCommand => _replyCommand;
    
    private readonly SharedBoolMutable _visible = new(false);
    private readonly SharedStringValue _currentText = new("");
    
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

    private readonly SyncCommand _replyCommand;

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

            foreach (var ro in _replyOptions)
            {
                ro.SetText("");
            }

            foreach (var rov in _replyOptionVisible)
            {
                rov.SetValue(false);
            }

            _currentText.SetText("");
            _visible.SetValue(false);

            _replyCommand.RaiseCanExecuteChanged();
            FocusElement?.Invoke(-1);
        });

        if (obj is int index)
        {
            OnReply(index);
        }
        else
        {
            OnReply(-1);
        }
    }

    protected override void SetValuesForDialog(string text, string[] replyOptions)
    {
        foreach (var ro in _replyOptions)
        {
            ro.SetText("");
        }

        foreach (var rov in _replyOptionVisible)
        {
            rov.SetValue(false);
        }

        for (var idx = 0; idx < replyOptions.Length; idx++)
        {
            _replyOptions[idx].SetText(replyOptions[idx]);
            _replyOptionVisible[idx].SetValue(true);
        }
        _visible.SetValue(true);
        
        _currentText.SetText("");
        _currentText.SetText(text);
        
        FocusElement?.Invoke(0);
        
        _replyCommand.RaiseCanExecuteChanged();
    }
}