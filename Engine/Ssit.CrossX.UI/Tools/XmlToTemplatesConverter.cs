using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ssit.CrossX.Xml;

namespace Ssit.CrossX.Tools;

public class XmlToTemplatesConverter(string fullPath, XNode xmlNode) : IXmlFileConverter
{
    public async Task Generate()
    {
        var outputFile = Path.Combine(Path.GetDirectoryName(fullPath) ?? "", Path.GetFileNameWithoutExtension(fullPath) ?? "") + $".cs";

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
        
        var @namespace = node.Attribute("Namespace");
        var className = Path.GetFileNameWithoutExtension(fullPath);
        
        Console.WriteLine($"Generating template class {className}...");
        
        var usingsBlock = string.Join("\n", usings) + "\n\n";
        
        var templates = XmlToCsConverter.GenerateTemplates(node, true);
        
        var nsLine = $"namespace {@namespace};\n\n";
        var classLine = $"public class {className}: TemplatesContainer\n{{";
        var contentBlock = string.Join("", templates);

        return usingsBlock + nsLine + classLine + contentBlock + "}\n";
    }
}