using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ssit.CrossX.Xml;

namespace Ssit.CrossX.Tools;

public static class XmlToCsConverter
{
    public static void GetUsings(XNode node, List<string> usings)
    {
        foreach (var cn in node.Nodes.Where(n => n.Tag == "xx_Using"))
        {
            var ns = cn.Value;
            if (!string.IsNullOrWhiteSpace(ns))
            {
                usings.Add($"using {ns};");
            }
        }
    }

    public static List<string> GenerateTemplates(XNode node, bool @public)
    {
        var list = new List<string>();
        
        foreach (var cn in node.Nodes.Where(n => n.Tag == "xx_Template"))
        {
            var ofType = cn.Attribute("xx_OfType");
            var name = cn.Attribute("xx_Name");
            var context = cn.Attribute("xx_Context");
            
            if(string.IsNullOrWhiteSpace(context)) context = "context";

            var children = GenerateChildren(cn, "\t");

            if (children.Count != 1) throw new InvalidDataException();

            var accessor = @public ? "public" : "private";
            
            var template = $"\n\t{accessor} View {name}({ofType} {context}) => {children[0].Substring(2)};\n";
            list.Add(template);
        }

        return list;
    }
    
    public static List<string> GenerateChildren(XNode node, string lineBegin)
    {
        var list = new List<string>();

        foreach (var cn in node.Nodes.Where(t => !t.Tag.StartsWith("xx_", StringComparison.InvariantCulture)))
        {
            var genericElement = "";
            var genericType = cn.Attribute("xx_OfType");
            if (!string.IsNullOrWhiteSpace(genericType))
            {
                genericElement = $"<{genericType}>";
            }
            
            var str = $"\n{lineBegin}new {cn.Tag}{genericElement}\n{lineBegin}{{\n";

            var attributes = GetAttributes(cn, lineBegin + "\t");
            var attachedAttributes = GetAttachedAttributes(cn, lineBegin + "\t");

            if (attachedAttributes.Count > 0)
            {
                attributes.AddRange(attachedAttributes);
            }
            
            var children = GenerateChildren(cn, lineBegin + "\t\t");
            
            if (children.Count > 0)
            {
                attributes.Add($"{lineBegin}\tChildren = [\n{lineBegin}\t\t" + string.Join($",\n{lineBegin}\t\t", children) + $"]");
            }

            str += string.Join($",\n", attributes);

            str += $"\n{lineBegin}}}";

            str += GetStyles(cn);
            
            list.Add(str);
        }

        return list;
    }

    public static string GetStyles(XNode node)
    {
        var styles = node.Attribute("xx_Styles");
        
        if (string.IsNullOrWhiteSpace(styles))
            return "";

        if (styles == "@")
        {
            return ".ApplyStyles(Styles)";
        }

        var line = ".ApplyStyles(Styles";
        
        foreach (var style in styles.Split(',').Select(s => s.Trim()))
        {
            line += ", ";
            line += '"' + style + '"';
        }

        line += ")";
        return line;
    }

    public static List<string> GetAttachedAttributes(XNode node, string lineBegin)
    {
        var list = new List<string>();
        
        foreach (var cn in node.Nodes.Where(t => t.Tag.StartsWith("xx_", StringComparison.InvariantCulture)))
        {
            switch (cn.Tag)
            {
                case "xx_AttachViewCreator":
                    list.Add(GenerateViewCreator(cn, lineBegin));
                    break;
            }
        }

        return list;
    }

    public static string GenerateViewCreator(XNode node, string lineBegin)
    {
        var property = node.Attribute("xx_Property");
        var children = GenerateChildren(node, lineBegin);
        var context = node.Attribute("xx_Context");

        if (string.IsNullOrWhiteSpace(context))
        {
            context = "context";
        }
        
        if (children.Count != 1) throw new InvalidDataException();

        return $"{lineBegin}{property} = {context} => {children[0]}";
    }

    public static List<string> GetAttributes(XNode node, string lineBegin)
    {
        var list = new List<string>();
        
        foreach (var attrName in node.Attributes)
        {
            if (attrName.StartsWith("xx_"))
                continue;
            
            var value = node.Attribute(attrName);
            if (value.StartsWith('{') && value.EndsWith('}'))
            {
                value = '"' + value.Substring(1, value.Length - 2) + '"';
            }
            
            list.Add(lineBegin + attrName + " = " + value);
        }

        return list;
    }
}