using System;
using System.Collections.Generic;
using System.IO;
using Ssit.CrossX.IO;
using Ssit.CrossX.Xml;

namespace Ssit.CrossX.Games.Logic.Narration;

internal static class NarrationParser
{
    public static IEnumerable<NarrationObject> ParseObjects(IFilesProvider filesProvider, string narrationDir)
    {
        var files = filesProvider.GetFiles(narrationDir, "xml");
    
        foreach (var file in files)
        {
            using var stream = filesProvider.Open(file);
            var obj = Parse(stream);
            if (obj != null)
            {
                yield return obj;
            }
        }
    }

    public static NarrationObject Parse(Stream stream)
    {
        var node = XNode.ReadXml(stream);

        if (node.Tag != "Dialogs")
            return null;

        var name = node.Attribute("Name");
        
        var dialogs = new List<NarrationDialog>();
        foreach (var cn in node.Nodes)
        {
            var dialog = ParseDialog(cn);
            if (dialog != null)
            {
                dialogs.Add(dialog);
            }
        }
        return new NarrationObject(name, dialogs);
    }

    private static NarrationDialog ParseDialog(XNode node)
    {
        if (node.Tag != "Dialog")
            return null;

        if (node.Nodes.Count != 1)
            throw new InvalidDataException();

        var highlight = node.Attribute("Highlight")?.ToLowerInvariant() == "true";
        var on = new HashSet<string>(node.Attribute("On")?.Split('|') ?? []);
        
        var entry = ParseEntry(node.Nodes[0]);
        if (entry == null)
            return null;
        
        return new NarrationDialog(on, highlight, entry);
    }

    private static NarrationEntry ParseEntry(XNode node)
    {
        if (node.Tag != "Text")
            return null;

        var text = node.Value.Trim('\n', ' ', '\t', '\r').Replace('|', '\n');
        var list = new List<NarrationOption>();

        foreach (var cn in node.Nodes)
        {
            var option = ParseOption(cn);
            if (option != null)
            {
                list.Add(option);
            }
        }

        if (list.Count == 0)
            return null;
        
        return new NarrationEntry(text, list.ToArray());
    }

    private static NarrationOption ParseOption(XNode node)
    {
        if (node.Tag != "Option")
            return null;
        
        var action = node.Attribute("Action");
        var set = node.Attribute("Set");
        var text = node.Value.Trim('\n', ' ', '\t', '\r');

        NarrationEntry entry = null;
        
        if(node.Nodes.Count > 1)
            throw new InvalidDataException();

        if (node.Nodes.Count == 1)
        {
            entry = ParseEntry(node.Nodes[0]);
        }
        
        return new NarrationOption(text, entry, action, set);
    }
}