using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Ssit.CrossX.Input.InputConfig;

namespace Nokemono.Core.Configuration;

public class Config
{
    public Palette[] Palettes { get; private set; }
    public MappingDesc InputMapping { get; private set; }
    public string PlayerName { get; private set; }

    public Config()
    {
        var location = Assembly.GetExecutingAssembly().Location;
        var dir = Path.GetDirectoryName(location);
        var path = Path.Combine(dir!, "Config.json");

        try
        {
            var text = File.ReadAllText(path, Encoding.UTF8);
            var data = JsonConvert.DeserializeObject<JsonConfig>(text);
            Fill(data);
        }
        catch (FileNotFoundException)
        {
            Default();
        }
    }

    private void Fill(JsonConfig data)
    {
        Palettes = new Palette[data.Palettes.Length];
        
        for(var idx = 0; idx < data.Palettes.Length; idx++)
        {
            var palette = data.Palettes[idx];
            Palettes[idx] = new Palette(palette.Name, palette.Colors, palette.ButtonAccentColor);
        }

        InputMapping = data.InputMapping;
        PlayerName = data.PlayerName;
    }

    private void Default()
    {
        Palettes = Palette.DefaultPalettes;
        PlayerName = "Lukas";
    }
}