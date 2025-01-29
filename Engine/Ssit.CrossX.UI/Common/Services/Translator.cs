using System.Collections.Generic;
using System.IO;
using SampleGame.Services;
using Ssit.CrossX.IO;
using Ssit.CrossX.UI.Values;

namespace Ssit.CrossX.Common.Services;

internal class Translator : ITranslator
{
    private readonly Dictionary<string, SharedStringValue> _strings = new();

    private readonly Dictionary<string, string>[] _languages;
    private int _language = 0;
    
    public Translator(IFilesProvider filesProvider)
    {
        using var stream = filesProvider.Open("assets:/Languages.tsv");
        var text = new StreamReader(stream).ReadToEnd();
        
        var lines = text.Split('\n', '\r');
        var list = new List<Dictionary<string, string>>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            ParseLine(line, list);
        }

        _languages = list.ToArray();
        UpdateLanguage(_languages[_language]);
    }

    private void ParseLine(string line, List<Dictionary<string, string>> list)
    {
        var parts = line.Split('\t');
        var key = parts[0];
        
        for(var idx = 1; idx < parts.Length; idx++)
        {
            var value = parts[idx];
            var listId = idx - 1;
            
            while (list.Count <= listId)
            {
                list.Add(new Dictionary<string, string>());
            }
            list[listId].Add(key, value);
        }
    }

    public void ToggleLanguage()
    {
        _language = (_language + 1) % 2;
        UpdateLanguage(_languages[_language]);
    }

    private void UpdateLanguage(Dictionary<string, string> dict)
    {
        foreach (var val in dict)
        {
            if (!_strings.TryGetValue(val.Key, out var str))
            {
                str = new SharedStringValue();
                _strings.Add(val.Key, str);
            }
            str.SetText(val.Value);
        }
    }

    public SharedString this[string key]
    {
        get
        {
            if (_strings.TryGetValue(key, out var str))
            {
                return str;
            }

            return key;
        }   
    }
}