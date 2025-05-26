using Ssit.CrossX;

namespace Nokemono.Core;

public class Palette
{
    public const byte Transparent = 0;
    public const byte Background = 1;
    public const byte Foreground = 2;
    public const byte Dim = 3;
    public const byte Accent = 4;
    public const byte ButtonAccent = 5;

    public static readonly Palette[] Palettes =
    [
        new("Default", RgbaColor.Transparent, RgbaColor.Black, RgbaColor.White, 0x606060, RgbaColor.Red, RgbaColor.Red),
        new("Soft", RgbaColor.Transparent, 0x201008, 0xddd0c0, 0x706050, 0xb06060, 0xb06060),
        new("Purple", RgbaColor.Transparent, 0x150413, 0xd0d0c1, 0x332431, 0x6e3c3c, 0x6e3c3c),
        new("Marron", RgbaColor.Transparent, 0x340011, 0xda7239, 0x781526, 0xcc4d36, 0x781526),
        new("Chocolate", RgbaColor.Transparent, 0x2b212c, 0xc09788, 0x65414b, 0x9b615c, 0x65414b),
        new("Amber", RgbaColor.Transparent, 0x201000, 0xfda813, 0x462800, 0xfd4713, 0xfd4713),
        new("Fav2", RgbaColor.Transparent, 0x101010, 0xd6d66e, 0x5b442f, 0xee5436, 0xee5436),
        new("Game Boy", RgbaColor.Transparent, 0x081820, 0x9cad91, 0x346856, 0x88c070, 0x346856),
        new("Game Boy + Red", RgbaColor.Transparent, 0x081820, 0x9cad91, 0x346856, 0xa05634, 0xa05634),
        new("Game Boy BW", RgbaColor.Transparent, 0x141414, 0x9f9f9f, 0x4e4e4e, 0x989898, 0x4e4e4e)
    ];
    
    public string Name { get; }
    public RgbaColor[] Colors { get; }
    
    public Palette(string name, params RgbaColor[] colors)
    {
        Name = name;
        Colors = colors;
    }
}