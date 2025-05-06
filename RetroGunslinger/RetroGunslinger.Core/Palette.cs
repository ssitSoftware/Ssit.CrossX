using Ssit.CrossX;

namespace RetroGunslinger.Core;

public class Palette
{
    public const byte Transparent = 0;
    public const byte Background = 1;
    public const byte Foreground = 2;
    public const byte Dim = 3;
    public const byte Accent = 4;

    public static readonly Palette[] Palettes =
    [
        new("Default", RgbaColor.Transparent, RgbaColor.Black, RgbaColor.White, RgbaColor.Gray, RgbaColor.Red),
        new("Soft", RgbaColor.Transparent, 0x201810, 0xd0c8c0, 0x807870, 0xa05050),
        new ("Purple", RgbaColor.Transparent, 0x150413, 0xffffd1, 0x332431, 0x6e3c3c),
        new("Cold", RgbaColor.Transparent, 0x08001f, 0xb2d5d1, 0x444d84, 0xb4643d),
        new("Marron", RgbaColor.Transparent, 0x340011, 0xf2b63f, 0x9c4012, 0xff0064),
        new("Chocolate", RgbaColor.Transparent, 0x3b313c, 0xedd8bd, 0x8b5760, 0xab313c),
        new("LCD", RgbaColor.Transparent, 0x101010, 0xa6f76e, 0x5b843f, 0xfe6a26)
    ];
    
    public string Name { get; }
    public RgbaColor[] Colors { get; }
    
    public Palette( string name, params RgbaColor[] colors )
    {
        Name = name;
        Colors = colors;
    }
}