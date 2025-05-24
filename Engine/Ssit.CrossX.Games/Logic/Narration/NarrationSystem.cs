using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.IO;

namespace Ssit.CrossX.Games.Logic.Narration;

public class NarrationSystem: INarrationSystem
{
    private readonly IGameDialogs _dialogs;
    private readonly IGameState _gameState;
    private readonly IActionScheduler _actionScheduler;
    public event Action<string> NarrationAction;
    public event Action NarrationUpdated;
    
    private readonly Dictionary<string, NarrationObject> _objects = new();
    
    public NarrationSystem(IGameDialogs dialogs, IGameState gameState, IActionScheduler actionScheduler, IFilesProvider filesProvider, string narrationDir)
    {
        _dialogs = dialogs;
        _gameState = gameState;
        _actionScheduler = actionScheduler;
        
        var list = NarrationParser.ParseObjects(filesProvider, narrationDir);
        foreach (var obj in list)
        {
            _objects.Add(obj.Name, obj);
        }
    }
    
    public async Task StartNarration(string subject)
    {
        var dialog = GetDialog(subject);
        if (dialog == null)
        {
            return;
        }

        var entry = dialog.Entry;

        while (entry != null)
        {
            var text = entry.Text.Replace("{playerName}", "Micah");
            var result = await _dialogs.ShowAsync(text, entry.Options.Select(o => o.Text.Replace("{playerName}", "Micah")).ToArray());
            
            var action = entry.Options[result].Action;
            var tag = entry.Options[result].SetTag;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                _gameState.SetFlag(tag);
                _actionScheduler.Schedule(() => NarrationUpdated?.Invoke());
            }

            if (!string.IsNullOrWhiteSpace(action))
            {
                _actionScheduler.Schedule(() => NarrationAction?.Invoke(action));
            }
            
            entry = entry.Options[result].Entry;
        }
        
        var key = GetDialogKey(subject, dialog.On);
        _gameState.SetFlag(key);

        _actionScheduler.Schedule(() => NarrationUpdated?.Invoke());
    }

    private NarrationDialog GetDialog(string subject)
    {
        if (!_objects.TryGetValue(subject, out var narrationObject))
        {
            return null;
        }
        
        foreach (var dialog in narrationObject.Dialogs.Reverse())
        {
            var valid = true;
            foreach (var tag in dialog.On)
            {
                if (!_gameState.HasFlag(tag))
                {
                    valid = false;
                }
            }

            if (valid)
            {
                return dialog;
            }
        }

        return null;
    }

    public bool HasRequest(string subject)
    {
        var dialog = GetDialog(subject);

        if (dialog.Highlight)
        {
            var key = GetDialogKey(subject, dialog.On);
            return !_gameState.HasFlag(key);
        }

        return false;
    }

    private string GetDialogKey(string subject, IEnumerable<string> ons)
    {
        return "shown-" + subject +":" + string.Join("|", ons); 
    }
}