using System;
using System.IO;
using Newtonsoft.Json;

namespace Ssit.CrossX.Editor.Models;

public class EditorData
{
    [JsonProperty] public string RecentMapPath { get; set; }

    private string _appName;
    
    public static EditorData Load(string appName)
    {
        var path = GetFilePath(appName);

        if (!File.Exists(path))
            return new EditorData
            {
                _appName = appName
            };
        
        
        using var stream = File.Open(path, FileMode.Open);
        using var streamReader = new StreamReader(stream);
        
        var json = streamReader.ReadToEnd();
        
        var data = JsonConvert.DeserializeObject<EditorData>(json);
        data._appName = appName;
        return data;
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(this);
        using var stream = File.Open(GetFilePath(_appName), FileMode.Create);
        using var streamWriter = new StreamWriter(stream);
        
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
        stream.Close();
    }

    private static string GetFilePath(string appName)
    {
        var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        dir = Path.Combine(dir, appName);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "data.json");
        return path;
    }
}