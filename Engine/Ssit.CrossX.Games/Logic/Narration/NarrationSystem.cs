using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Core;
using Ssit.CrossX.IO;
using Ssit.CrossX.UI.Common.Services;

namespace Ssit.CrossX.Games.Logic.Narration;

public class NarrationSystem: INarrationSystem
{
    private readonly IGameDialogs _dialogs;
    private readonly IGameState _gameState;
    private readonly IActionScheduler _actionScheduler;
    private readonly ITranslator _translator;
    public event Action<string> NarrationAction;
    
    private readonly Dictionary<string, NarrationObject> _objects = new();
    
    public NarrationSystem(IGameDialogs dialogs, IGameState gameState, IActionScheduler actionScheduler, IFilesProvider filesProvider, ITranslator translator, string narrationDir)
    {
        _dialogs = dialogs;
        _gameState = gameState;
        _actionScheduler = actionScheduler;
        _translator = translator;

        var list = NarrationParser.ParseObjects(filesProvider, narrationDir);
        foreach (var obj in list)
        {
            _objects.Add(obj.Name, obj);
        }
    }

    private string GetText(Dictionary<string, string> dict, string langId, string defaultId)
    {
        if (!dict.TryGetValue(langId, out var text))
        {
            text = dict.GetValueOrDefault(defaultId, "????");
        }
        text = text.Replace("{playerName}", "Micah");
        return text;
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
            bool reload = true;
            int result = 0;
            
            while (reload)
            {
                var langId = _translator["#LangId"].ToString();

                var text = GetText(entry.Text, langId, dialog.DefaultLanguage);
                
                result = await _dialogs.ShowAsync(text,
                    entry.Options.Select(o => GetText(o.Text, langId, dialog.DefaultLanguage)).ToArray());
                
                reload = result < 0;
            }

            var action = entry.Options[result].Action;
            var tag = entry.Options[result].SetTag;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                _gameState.SetFlag(tag);
            }

            if (!string.IsNullOrWhiteSpace(action))
            {
                _actionScheduler.Schedule(() => NarrationAction?.Invoke(action));
            }
            
            entry = entry.Options[result].Entry;
        }
        
        var key = GetDialogKey(subject, dialog.On);
        _gameState.SetFlag(key);
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