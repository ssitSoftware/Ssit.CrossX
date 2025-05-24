using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ssit.CrossX.Xml;

namespace Ssit.CrossX.Tools;

public class XmlToPageConverter(string fullPath, XNode xmlNode) : IXmlFileConverter
{
    public async Task Generate()
    {
        var ext = xmlNode.Attribute("xx_Partial")?.ToLowerInvariant() == "true" ? ".g" : "";
        var outputFile = Path.Combine(Path.GetDirectoryName(fullPath) ?? "", Path.GetFileNameWithoutExtension(fullPath) ?? "") + $"{ext}.cs";

        File.Delete(outputFile);
        await Task.Delay(10);
        
        await using var outputStream = File.Open(outputFile, FileMode.Create, FileAccess.Write);
        await using var streamWriter = new StreamWriter(outputStream);

        var generatedFile = await Task.Run(() => Convert(xmlNode));
        generatedFile = generatedFile.Replace("\t", "    ");
        await streamWriter.WriteAsync(generatedFile);
        await streamWriter.FlushAsync();
    }

    private string Convert(XNode node)
    {
        List<string> usings = new List<string>();
        XmlToCsConverter.GetUsings(node, usings);
        
        string root = GenerateRoot(node);
        
        var @namespace = node.Attribute("Namespace");
        var viewModel = node.Attribute("ViewModel");
        
        var className = Path.GetFileNameWithoutExtension(fullPath);
        
        Console.WriteLine($"Generating page class {className}...");
        
        var usingsBlock = string.Join("\n", usings) + "\n\n";
        
        var templates = XmlToCsConverter.GenerateTemplates(node, false);
        
        var partialKeyword = node.Attribute("xx_Partial")?.ToLowerInvariant() == "true" ? "partial " : "";

        var containers = GetContainers(node, "\t\t");
        var containersLine = "";

        if (containers.Count > 0)
        {
            containersLine = string.Join("\n", containers) + "\n\n";
        }
        
        var nsLine = $"namespace {@namespace};\n\n";
        var classLine = $"public {partialKeyword}class {className}: Page<{viewModel}>\n{{\n";
        var contentBlock = $"\tprotected override View CreateView()\n\t{{\n{containersLine}\t\treturn {root}\n\t}}";

        if (templates.Count > 0)
        {
            contentBlock += "\n" + string.Join("\n", templates);
        }

        return usingsBlock + nsLine + classLine + contentBlock + "\n}\n";
    }

    private List<string> GetContainers(XNode node, string lineStart)
    {
        var list = new List<string>();

        foreach (var cn in node.Nodes.Where(o => o.Tag == "xx_ImportContainer"))
        {
            var type = cn.Attribute("xx_Type");
            var name = cn.Attribute("xx_Name");

            var line = $"{lineStart}var {name} = GetContainer<{type}>();";
            list.Add(line);
        }

        return list;
    }

    private string GenerateRoot(XNode node)
    {
        var children = XmlToCsConverter.GenerateChildren(node, "\t\t");

        if (children.Count != 1)
        {
            throw new InvalidOperationException();
        }

        return children[0].Substring(3) + ";";
    }
}